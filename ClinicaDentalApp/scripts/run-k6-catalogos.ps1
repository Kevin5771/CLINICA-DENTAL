param(
    [string]$BaseUrl = "http://localhost:5000",
    [string]$ApiUser = "admin",
    [string]$ApiPassword = "Admin123*"
)

Write-Host "Ejecutando prueba k6 de catálogos..." -ForegroundColor Cyan
k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword .\k6-tests\02-catalogos.js
