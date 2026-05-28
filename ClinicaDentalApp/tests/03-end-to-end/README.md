# 03 - Pruebas end-to-end

Este apartado tiene dos caminos:

## A. End-to-end Web con .NET

Proyecto:

```text
ClinicaDental.Web.E2ETests
```

Comando:

```powershell
dotnet test ClinicaDental.Web.E2ETests/ClinicaDental.Web.E2ETests.csproj
```

Valida flujo web: login, acceso a pacientes y mensajes de error.

## B. End-to-end API con k6

Carpeta:

```text
k6-tests
```

Comando recomendado para empezar:

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/01-login.js
```

Después:

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/05-flujo-completo-api.js
```

Valida flujo real contra la API levantada: login, JWT, pacientes, citas, reportes y carga básica.
