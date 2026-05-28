# Pruebas End-to-End y carga con k6

Estas pruebas ejecutan el flujo real contra la API de la clínica dental. A diferencia de las pruebas unitarias e integración, aquí debes tener la API levantada y conectada a la base de datos.

## Requisitos

1. Tener instalado k6.
2. Tener levantada la API `ClinicaDental.Api`.
3. Tener un usuario válido para iniciar sesión.
4. Verificar que `BASE_URL` apunte al puerto correcto de la API.

## Variables usadas

| Variable | Valor por defecto | Descripción |
|---|---:|---|
| `BASE_URL` | `http://localhost:5000` | URL base de la API |
| `API_USER` | `admin` | Usuario para login |
| `API_PASSWORD` | `Admin123*` | Contraseña para login |
| `VUS` | `5` | Usuarios virtuales para carga básica |

## Ejecutar prueba por prueba

Desde la raíz del proyecto:

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/01-login.js
```

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/02-catalogos.js
```

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/03-pacientes-readonly.js
```

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/04-citas-readonly.js
```

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* k6-tests/05-flujo-completo-api.js
```

```powershell
k6 run -e BASE_URL=http://localhost:5000 -e API_USER=admin -e API_PASSWORD=Admin123* -e VUS=5 k6-tests/06-carga-basica-api.js
```

## Qué prueba cada archivo

| Archivo | Tipo | Qué valida |
|---|---|---|
| `01-login.js` | Smoke / autenticación | Login y generación de token JWT |
| `02-catalogos.js` | End-to-end API | Catálogos protegidos con JWT |
| `03-pacientes-readonly.js` | End-to-end API | Listado y detalle de pacientes |
| `04-citas-readonly.js` | End-to-end API | Listado y detalle de citas |
| `05-flujo-completo-api.js` | End-to-end API real | Login, crear paciente, crear cita, consultar resumen |
| `06-carga-basica-api.js` | Carga básica | Múltiples usuarios virtuales consultando endpoints principales |

## Nota importante

El archivo `05-flujo-completo-api.js` crea datos reales de prueba en la base de datos. Usa códigos con prefijo `K6P` para que puedas identificarlos.

