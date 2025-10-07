Write-Host "StarWrite-Host "Backend API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host "MySQL: localhost:3307" -ForegroundColor Cyang Hotel Booking Service in Docker..." -ForegroundColor Green

Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose down

Write-Host "Building and starting containers..." -ForegroundColor Yellow
docker-compose up --build -d

Write-Host "Waiting for database startup..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

Write-Host "Container status:" -ForegroundColor Cyan
docker-compose ps

Write-Host ""
Write-Host "Backend services started successfully!" -ForegroundColor Green
Write-Host "Backend API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host "MySQL: localhost:3307" -ForegroundColor Cyan
Write-Host ""
Write-Host "To start frontend run:" -ForegroundColor Yellow
Write-Host "   cd Front\HotelServiceFE" -ForegroundColor White
Write-Host "   npm install" -ForegroundColor White
Write-Host "   ng serve" -ForegroundColor White
Write-Host ""
Write-Host "Check logs with: docker-compose logs [service-name]" -ForegroundColor White
Write-Host "Stop project: docker-compose down" -ForegroundColor White