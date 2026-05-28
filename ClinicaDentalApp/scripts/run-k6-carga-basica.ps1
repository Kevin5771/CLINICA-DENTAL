param(
    [string]$BaseUrl = "http://localhost:5000",
    [string]$ApiUser = "admin",
    [string]$ApiPassword = "Admin123*",
    [int]$Vus = 5
)

Write-Host "Ejecutando prueba k6 de carga básica..." -ForegroundColor Cyan
k6 run -e BASE_URL=$BaseUrl -e API_USER=$ApiUser -e API_PASSWORD=$ApiPassword -e VUS=$Vus .\k6-tests\06-carga-basica-api.js
