# Set de pruebas del proyecto Clínica Dental

El set quedó dividido por tipo para poder identificarlo y ejecutarlo individualmente:

```text
01-unitarias
02-integracion
03-end-to-end
```

## 1. Pruebas unitarias

Proyecto real:

```text
ClinicaDental.Api.UnitTests
```

Ejecutar:

```powershell
dotnet test ClinicaDental.Api.UnitTests/ClinicaDental.Api.UnitTests.csproj
```

Estas pruebas validan controladores y servicios de forma aislada, sin levantar API real y sin SQL Server.

## 2. Pruebas de integración

Proyecto real:

```text
ClinicaDental.Api.IntegrationTests
```

Ejecutar:

```powershell
dotnet test ClinicaDental.Api.IntegrationTests/ClinicaDental.Api.IntegrationTests.csproj
```

Estas pruebas levantan la API en memoria con `WebApplicationFactory` y prueban endpoints HTTP protegidos con JWT usando repositorios falsos.

## 3. Pruebas end-to-end

Hay dos apartados:

### 3.1 End-to-end web con .NET

Proyecto real:

```text
ClinicaDental.Web.E2ETests
```

Ejecutar:

```powershell
dotnet test ClinicaDental.Web.E2ETests/ClinicaDental.Web.E2ETests.csproj
```

Estas pruebas verifican el flujo web de login y navegación.

### 3.2 End-to-end API con k6

Carpeta real:

```text
k6-tests
```

Ejecutar login:

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/01-login.js
```

Ejecutar flujo completo:

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/05-flujo-completo-api.js
```

Estas pruebas sí golpean la API real. Para usarlas, la API debe estar levantada y la base de datos debe estar funcionando.

