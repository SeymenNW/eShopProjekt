# =====================================================
# eShopProject - Start script for Basket microservice
# =====================================================

Write-Host "---- eShopProject: Starting dependencies ----" -ForegroundColor Cyan

# 1. Start PostgreSQL and RabbitMQ in background
docker compose up -d basket-db rabbitmq

# 2. Vent lidt så services starter
Write-Host "Waiting 25 seconds for PostgreSQL and RabbitMQ to initialize..."
Start-Sleep -Seconds 25

# 3. Kør EF migrations automatisk (kun hvis database mangler tabeller)
Write-Host "Checking and applying EF migrations..."
dotnet ef database update -p eShop.Basket.Infrastructure -s eShop.Basket.API

# 4. Start Basket API
Write-Host "Starting Basket.API..."
dotnet run --project eShop.Basket.API

Write-Host "---------------------------------------------"
Write-Host "Basket microservice running at http://localhost:8081"
Write-Host "Swagger: http://localhost:8081/swagger"
Write-Host "---------------------------------------------"
