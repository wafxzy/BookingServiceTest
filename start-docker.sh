
echo "Starting Hecho "Backend API: http://localhost:5000"
echo "Swagger UI: http://localhost:5000/swagger"
echo "MySQL: localhost:3307"l Booking Service in Docker..."

echo "Stopping existing containers..."
docker-compose down

echo "Building and starting containers..."
docker-compose up --build -d

echo "Waiting for database startup..."
sleep 30

echo "Container status:"
docker-compose ps

echo ""
echo "Backend services started successfully!"
echo " Backend API: http://localhost:5000"
echo "Swagger UI: http://localhost:5000/swagger"
echo "MySQL: localhost:3307"
echo ""
echo "To start frontend run:"
echo "   cd Front/HotelServiceFE"
echo "   npm install"
echo "   ng serve"
echo ""
echo "Check logs with: docker-compose logs [service-name]"
echo "Stop project: docker-compose down"