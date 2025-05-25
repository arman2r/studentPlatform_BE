#!/bin/bash

# Configuración de variables
DB_NAME="EducationDB"
BACKUP_FILE="/var/opt/mssql/backup/EducationAPIDB.bak"
SA_USER="sa"
SA_PASSWORD="${SA_PASSWORD:-Admin123--}"
MAX_RETRIES=60
RETRY_INTERVAL=2

# Función para esperar a que SQL Server esté realmente listo
wait_for_sql() {
    echo "Esperando que SQL Server esté completamente listo..."
    local i=0
    while [ $i -lt $MAX_RETRIES ]; do
        if sqlcmd -S localhost -U $SA_USER -P "$SA_PASSWORD" -Q "SELECT 1" &> /dev/null; then
            # Verificación adicional - esperar a que las bases de sistema estén listas
            if sqlcmd -S localhost -U $SA_USER -P "$SA_PASSWORD" -Q "SELECT name FROM sys.databases WHERE state = 0" | grep -q "master"; then
                echo "SQL Server está completamente operativo."
                return 0
            fi
        fi
        sleep $RETRY_INTERVAL
        i=$((i+1))
        echo "Intento $i/$MAX_RETRIES - SQL Server aún no está listo..."
    done
    echo "Error: SQL Server no respondió adecuadamente después de $((MAX_RETRIES * RETRY_INTERVAL)) segundos"
    exit 1
}

# Esperar a que SQL Server esté realmente listo
wait_for_sql

# Verificar si la base ya existe
if sqlcmd -S localhost -U $SA_USER -P "$SA_PASSWORD" -Q "SELECT name FROM sys.databases WHERE name = '$DB_NAME'" | grep -q "$DB_NAME"; then
    echo "La base de datos $DB_NAME ya existe. Se omite la restauración."
    exit 0
fi

# 2. Restaurar la base de datos desde el backup
echo "Restaurando base de datos desde el backup..."
sqlcmd -S localhost -U $SA_USER -P "$SA_PASSWORD" -Q "
RESTORE DATABASE [$DB_NAME] 
FROM DISK = '$BACKUP_FILE'
WITH 
    MOVE '${DB_NAME}_Data' TO '/var/opt/mssql/data/${DB_NAME}.mdf',
    MOVE '${DB_NAME}_Log' TO '/var/opt/mssql/data/${DB_NAME}_log.ldf',
    REPLACE,
    STATS = 5
" || {
    echo "Error al restaurar la base de datos"
    exit 1
}

echo "Proceso de restauración completado exitosamente."
exit 0