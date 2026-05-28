param(
    [string]$BaseUrl = "http://localhost:5000",
    [string]$ApiUser = "admin",
    [string]$ApiPassword = "Admin123*",
    [int]$Vus = 5
)

Write-Host "Ejecutando pruebas k6 una por una..." -ForegroundColor Cyan

k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\01-login.js
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\02-catalogos.js
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\03-pacientes-readonly.js
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\04-citas-readonly.js
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\05-flujo-completo-api.js
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword -e VUS=$Vus .\k6-tests\06-carga-basica-api.js
