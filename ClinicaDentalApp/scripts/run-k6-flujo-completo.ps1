param(
    [string]$BaseUrl = "http://localhost:5000",
    [string]$ApiUser = "admin",
    [string]$ApiPassword = "Admin123*"
)

Write-Host "Ejecutando flujo completo k6 contra la API real..." -ForegroundColor Cyan
Write-Host "Aviso: esta prueba crea un paciente y una cita de prueba." -ForegroundColor Yellow
k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\05-flujo-completo-api.js
