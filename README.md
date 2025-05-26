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
#### **Importante**: Este paso solo aplica si vas a ejecutar el proyecto sin docker

Antes de iniciar el backend, asegúrate de **crear la base de datos manualmente** en SQL Server (por ejemplo desde SSMS).  
Si decides usar otro nombre o instancia, recuerda **actualizar la cadena de conexión** en:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=EducationDb;User Id=susuario;Password=YourStrong!Pass123;"
}

```

### 1.2. Ejecutar la migración inicial
- **Si has creado la base de datos manualmente, asegúrate de ejecutar las migraciones iniciales para crear las tablas necesarias. Puedes hacer esto ejecutando el siguiente comando en la terminal:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
- Esto creará la estructura inicial de la base de datos y aplicará las migraciones necesarias.

## 1.3. Poblar la base de datos con datos de ejemplo
- Ejecuta el proyecto desde visual studio y apoyate con swagger para registrar profesores, estudiantes y materias(Subjects)
- El proyecto tambien incluye un script de migración y un backup de la base de datos dentro de la carpeta mssql


### 2. Clonar el repositorio y Ejecutar con Docker
- **Asegúrate de tener Docker Desktop instalado y en funcionamiento.**
- Este paso es el mas recomendado y te ahorra tiempo en configuración y dependencias.
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