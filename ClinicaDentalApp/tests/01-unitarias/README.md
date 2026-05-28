# 01 - Pruebas unitarias

## Proyecto

```text
ClinicaDental.Api.UnitTests
```

## Comando

```powershell
dotnet test ClinicaDental.Api.UnitTests/ClinicaDental.Api.UnitTests.csproj
```

## Qué validan

- `PasswordService`
- `JwtTokenService`
- `PacientesController`
- `CitasController`
- `StockController`
- `VentasController`

## Característica principal

No dependen de SQL Server ni de una API levantada. Son rápidas y prueban partes aisladas del sistema.
