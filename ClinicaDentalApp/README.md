# ClinicaDentalApp

Base inicial del proyecto para clínica dental con:

- API REST en ASP.NET Core
- Cliente web en Razor Pages
- SQL Server como base de datos
- Autenticación básica con JWT en la API
- Consumo de API desde Razor Pages

## Qué incluye esta primera entrega

### API (`ClinicaDental.Api`)
- Login de usuarios
- CRUD inicial de pacientes
- CRUD inicial de citas
- CRUD inicial de usuarios
- Catálogo de roles
- Catálogo de estados de cita
- Validaciones básicas alineadas con tus RF
- Uso de stored procedures existentes de tu BD cuando ya están definidos

### Web (`ClinicaDental.Web`)
- Login
- Dashboard simple
- Listado y registro de pacientes
- Listado y registro de citas

## Estructura

- `ClinicaDental.Api/` API REST
- `ClinicaDental.Web/` cliente Razor Pages
- `database/` script SQL base

## Requisitos para ejecutar

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 o VS Code

## Base de datos

Tu script original fue tomado como base. En esta entrega se asume que ya existe la base `ClinicaDentalAppDB` con sus tablas y procedimientos.

Archivo incluido:
- `database/dentalapp_utf8.sql`

> Nota: el script original exportado por SQL Server trae rutas físicas del MDF/LDF. Si da problemas al crear la base en otra máquina, lo mejor es abrir el script y adaptar esa parte o crear primero la base vacía y luego ejecutar solo la parte de tablas, constraints, catálogos y procedimientos.

## Configuración de conexión

Edita estos archivos:

### API
`ClinicaDental.Api/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=ClinicaDentalAppDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### Web
`ClinicaDental.Web/appsettings.json`

```json
"ApiSettings": {
  "BaseUrl": "https://localhost:7001/"
}
```

## Cómo ejecutar

### 1. API
Desde la carpeta `ClinicaDental.Api`:

```bash
dotnet restore
dotnet run
```

### 2. Web
Desde la carpeta `ClinicaDental.Web`:

```bash
dotnet restore
dotnet run
```

## Credenciales
No se incluyen credenciales quemadas.
Debes usar un usuario que ya exista en tu BD y cuya contraseña haya sido almacenada usando el mismo esquema de hash PBKDF2 que implementé en la API al crear usuarios nuevos.

## Importante
No pude compilar ni probar la solución dentro de este entorno porque aquí no está instalado `.NET SDK`. Aun así, te dejo la estructura completa lista para abrir en Visual Studio, restaurar paquetes y correrla en tu máquina.

## Siguiente paso recomendado
Cuando la pruebes, en este mismo chat me dices:
- qué parte quieres ampliar
- si quieres más páginas Razor
- si quieres JWT con autorización por roles
- si quieres historiales clínicos, ventas, inventario o reportes

