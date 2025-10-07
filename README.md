HotelBooking test project from Oleksandr

istructions hw to start 
if you would open be from vs make sure your creds in appsettings are right 
only BE and DB in docker 

## Requirements

- Docker Desktop
- Docker Compose
- Node.js and npm (for frontend)

### 1. Start backend and database

In the project root folder run:

```powershell
docker-compose up --build -d
```
### 2. Start frontend

Open a new terminal and run:

```powershell
cd Front/HotelServiceFE
npm install
ng serve
```

### 3. Done

- **Application**: http://localhost:4200
- **API**: http://localhost:5000

## Admin credentials

- **Email**: `admin@g.com`
- **Password**: `Admin123!`

## Stop application

```powershell
# Stop Docker containers
docker-compose down


### if  Port 3307 is busy
```powershell
# If you already have MySQL installed, change port in docker-compose.yml
# from 3307 to another port (e.g., 3308)
```

### npm errors
```powershell
cd Front/HotelServiceFE
npm cache clean --force
npm install
```

### Docker won't start
- Make sure Docker Desktop is running

## Application features

- User registration and authentication
- Hotel search and booking
- Booking management
- Admin panel with statistics
- Booking cancellation

## Technical information

- **Frontend**: Angular 21+ with Tailwind CSS
- **Backend**: ASP.NET Core 9 with Entity Framework
- **Database**: MySQL 8.0
- **Authentication**: JWT Bearer tokens + Identity
- **Containerization**: Docker + Docker Compose