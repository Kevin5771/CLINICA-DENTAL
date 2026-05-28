# 02 - Pruebas de integración

## Proyecto

```text
ClinicaDental.Api.IntegrationTests
```

## Comando

```powershell
dotnet test ClinicaDental.Api.IntegrationTests/ClinicaDental.Api.IntegrationTests.csproj
```

## Qué validan

- Login contra endpoint HTTP.
- Token JWT.
- Endpoints protegidos.
- Listado de pacientes.
- Creación de pacientes.
- Catálogos.

## Característica principal

Usan `WebApplicationFactory`, por lo tanto prueban la API como aplicación HTTP en memoria, pero no dependen de SQL Server.
