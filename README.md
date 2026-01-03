# Healthcare Management System

This is a full-stack app for managing patients, caregivers, and offices. It's built with .NET 8 on the backend and Angular on the frontend.

## Getting Started

### 1. Spin up the database and cache
I've included a docker-compose file to get PostgreSQL and Redis running quickly.
```bash
cd infra
docker-compose up -d
```

### 2. Run the Backend
You'll need the .NET 8 SDK. Run these commands to get the API up and running:
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```
The API will be at `http://localhost:5000`. You can check out the Swagger docs at `http://localhost:5000/swagger`.

### 3. Run the Frontend
Standard Angular setup here:
```bash
cd frontend
npm install
npm start
```
The app will open at `http://localhost:4200`.

---

## Interview Questions

### 1. Architecture
Angular + .NET 8 API. It follows a layered pattern (Controller -> Service -> Repository) with PostgreSQL for storage and Redis for caching.

### 2. Search Optimization
Fast searches (<300ms) via DB indexing, optimized EF Core queries, Redis caching, and pagination.

### 3. Auth & Auditing
- **Auth**: Uses custom middleware for role-based access. In production, I'd use standard JWT claim-based authorization within the ASP.NET Core framework.
- **Auditing**: Custom middleware logs request details and speed to the DB. For a real app, I'd use a logging framework like Serilog for structured logging and better performance.

### 4. Caching
Redis "cache-aside" pattern. Results are cached for 5-10 mins and invalidated automatically on any data updates.
