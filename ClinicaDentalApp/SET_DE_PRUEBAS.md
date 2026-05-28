# Set de pruebas - Clínica Dental

El proyecto incluye un set de pruebas dividido por apartados para que se puedan identificar y ejecutar de forma individual:

```text
1. Pruebas unitarias
2. Pruebas de integración
3. Pruebas end-to-end
   3.1 End-to-end web con .NET
   3.2 End-to-end API y carga con k6
```

Además se agregó la carpeta `scripts/` con comandos listos para Windows/PowerShell.

---

## 1. Pruebas unitarias

Proyecto:

```text
ClinicaDental.Api.UnitTests
```

Comando directo:

```powershell
dotnet test ClinicaDental.Api.UnitTests/ClinicaDental.Api.UnitTests.csproj
```

Comando con script:

```powershell
.\scripts\run-unitarias.ps1
```

Cobertura principal:

- `PasswordService`: generación y verificación de contraseñas con hash/salt.
- `JwtTokenService`: generación de JWT con claims esperados.
- `PacientesController`: validaciones de fecha de nacimiento, teléfono y nombres.
- `CitasController`: validaciones de fecha pasada y rango de horas.
- `StockController`: validaciones de productos, stock y movimientos.
- `VentasController`: validación de usuario autenticado y datos de venta.

Estas pruebas no usan SQL Server. Usan repositorios falsos en memoria para revisar la lógica de los controladores y servicios.

---

## 2. Pruebas de integración

Proyecto:

```text
ClinicaDental.Api.IntegrationTests
```

Comando directo:

```powershell
dotnet test ClinicaDental.Api.IntegrationTests/ClinicaDental.Api.IntegrationTests.csproj
```

Comando con script:

```powershell
.\scripts\run-integracion.ps1
```

Cobertura principal:

- Endpoint raíz `/` de la API.
- Seguridad JWT en endpoints protegidos.
- Login real contra el pipeline de la API usando un usuario falso en memoria.
- Consulta de pacientes autenticada.
- Creación de paciente autenticada.
- Catálogos protegidos por token.

Estas pruebas levantan la API con `WebApplicationFactory<Program>` y reemplazan los repositorios reales por repositorios en memoria. Por eso no necesitan una base de datos real para ejecutarse.

Credenciales usadas por la API de integración:

```text
Usuario: admin
Contraseña: Admin123
```

---

## 3. Pruebas end-to-end

### 3.1 End-to-end web con .NET

Proyecto:

```text
ClinicaDental.Web.E2ETests
```

Comando directo:

```powershell
dotnet test ClinicaDental.Web.E2ETests/ClinicaDental.Web.E2ETests.csproj
```

Comando con script:

```powershell
.\scripts\run-e2e-dotnet.ps1
```

Cobertura principal:

- Usuario anónimo entra a `/` y es redirigido a `/Auth/Login`.
- Login completo desde Razor Pages usando token antiforgery real.
- Después del login, el usuario puede abrir `/Pacientes` y ver datos renderizados.
- Login incorrecto se queda en la pantalla de login y muestra mensaje de error.

Estas pruebas levantan el proyecto web con `WebApplicationFactory<Program>` y simulan una API falsa mediante `HttpMessageHandler`. Así se prueba el flujo completo de navegación web sin depender de SQL Server ni de que la API real esté corriendo.

### 3.2 End-to-end API y carga con k6

Carpeta:

```text
k6-tests
```

Archivos incluidos:

| Archivo | Qué valida |
|---|---|
| `01-login.js` | Login real contra la API y token JWT |
| `02-catalogos.js` | Catálogos protegidos por JWT |
| `03-pacientes-readonly.js` | Listado y detalle de pacientes |
| `04-citas-readonly.js` | Listado y detalle de citas |
| `05-flujo-completo-api.js` | Login, creación de paciente, creación de cita y resúmenes |
| `06-carga-basica-api.js` | Carga básica con usuarios virtuales |

Comando inicial recomendado:

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/01-login.js
```

Comando con script:

```powershell
.\scripts\run-k6-login.ps1 -BaseUrl "http://localhost:5000" -ApiUser "admin" -ApiPassword "Admin123*"
```

Flujo completo con k6:

```powershell
.\scripts\run-k6-flujo-completo.ps1 -BaseUrl "http://localhost:5000" -ApiUser "admin" -ApiPassword "Admin123*"
```

Carga básica:

```powershell
.\scripts\run-k6-carga-basica.ps1 -BaseUrl "http://localhost:5000" -ApiUser "admin" -ApiPassword "Admin123*" -Vus 5
```

Nota: las pruebas k6 sí necesitan que la API real esté levantada y que exista un usuario válido en la base de datos.

---

## Ejecutar todo el set .NET

Desde la carpeta donde está `ClinicaDentalApp.sln`:

```powershell
dotnet restore
dotnet test ClinicaDentalApp.sln
```

O con script:

```powershell
.\scripts\run-dotnet-tests.ps1
```

## Ejecutar todo k6

Con la API real levantada:

```powershell
.\scripts\run-k6-todo.ps1 -BaseUrl "http://localhost:5000" -ApiUser "admin" -ApiPassword "Admin123*" -Vus 5
```

## Nota importante

Para que `WebApplicationFactory<Program>` funcione, se agregó al final de ambos `Program.cs`:

```csharp
public partial class Program { }
```

Esto es normal en proyectos ASP.NET Core cuando se quieren probar aplicaciones con top-level statements.

También se ajustó la API para no aplicar `UseHttpsRedirection()` cuando el ambiente es `Testing`, evitando redirecciones innecesarias dentro del servidor de pruebas.
