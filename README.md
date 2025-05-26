# EducationAPI - Proyecto .NET Core 8 + SQL Server

Este proyecto es una API construida con **.NET Core 8** y utiliza **SQL Server** como sistema de gestión de bases de datos. El entorno está preparado para ejecutarse en **contenedores Docker**.

---

## 🛠 Requisitos del sistema

- **.NET SDK 8.0**
- **Docker Desktop** instalado y funcionando
- **SQL Server Management Studio (SSMS)** o cliente SQL equivalente

### 📦 Detalles de versiones
| Componente                            | Versión                  |
|--------------------------------------|--------------------------|
| .NET Framework                       | 4.0.30319.42000          |
| SQL Server Management Studio (SSMS)  | 19.3.4.0                 |
| SQL Server Management Objects (SMO)  | 16.200.48053.0           |
| T-SQL Parser                         | 17.0.27.0                |
| Análisis Services Client Tools       | 16.0.20054.0             |
| MSXML                                | 3.0, 6.0                 |
| SO                                   | Windows 10.0.26100       |

---

## 🧩 Configuración inicial

### 1. Crear la base de datos

Antes de iniciar el backend, asegúrate de **crear la base de datos manualmente** en SQL Server (por ejemplo desde SSMS).  
Si decides usar otro nombre o instancia, recuerda **actualizar la cadena de conexión** en:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=EducationDb;User Id=susuario;Password=YourStrong!Pass123;"
}

```

### 2. Clonar el repositorio
```bash
git clone https://github.com/arman2r/studentPlatform_BE.git
```

### 3. Ejecutar el contenedor Docker
```bash
docker compose up --build
```
- **Esto puede demorar alrededor de 10 minutos la primera vez, dependiendo de tu conexión a internet y recursos del sistema (memoria, CPU, disco).

### 4. Acceder a la API
Una vez que el contenedor esté en funcionamiento, la API estará disponible en:
```bash
http://localhost:8080/swagger/index.html
```

### 6. Probar la API
- ** Puedes utilizar herramientas como **Postman** o **Insomnia** para probar los endpoints de la API. La documentación de los endpoints está disponible en Swagger.
- ** El proyecto incluye un script de migración que precarga algunos datos de ejemplo, esto le permiti interactuar directamente con las API o el aplicativo frontend directamente


### 7. Migraciones y Seed Data
Para aplicar migraciones y cargar datos de ejemplo, puedes ejecutar el siguiente comando en la terminal dentro del contenedor:
```bash
dotnet ef database update
```