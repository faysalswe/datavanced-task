# Infrastructure Services

This directory contains Docker Compose configuration for running infrastructure services locally.

## Services

- **PostgreSQL 16**: Main database for the healthcare application
- **Redis 7**: Caching and session management
- **pgAdmin** (optional): PostgreSQL management UI
- **Redis Commander** (optional): Redis management UI

## Quick Start

### Start Core Services (PostgreSQL & Redis)
```bash
cd infra
docker compose up -d
```

### Start with Management Tools
```bash
docker compose --profile tools up -d
```

### Stop Services
```bash
docker compose down
```

### Stop and Remove Volumes (Clean State)
```bash
docker compose down -v
```

## Service Details

### PostgreSQL
- **Port**: 5432
- **Database**: healthcare_db
- **User**: healthcare_user
- **Password**: healthcare_pass
- **Connection String**: `Host=localhost;Port=5432;Database=healthcare_db;Username=healthcare_user;Password=healthcare_pass`

### Redis
- **Port**: 6379
- **Password**: redis_pass
- **Connection String**: `localhost:6379,password=redis_pass`

### pgAdmin (Optional)
- **URL**: http://localhost:5050
- **Email**: admin@healthcare.local
- **Password**: admin

To connect to PostgreSQL in pgAdmin:
- Host: postgres
- Port: 5432
- Database: healthcare_db
- Username: healthcare_user
- Password: healthcare_pass

### Redis Commander (Optional)
- **URL**: http://localhost:8081

## Health Checks

Check service health:
```bash
docker compose ps
```

View logs:
```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f postgres
docker compose logs -f redis
```

## Data Persistence

Data is persisted in Docker volumes:
- `postgres_data`: PostgreSQL data
- `redis_data`: Redis data

To backup data:
```bash
# PostgreSQL backup
docker compose exec postgres pg_dump -U healthcare_user healthcare_db > backup.sql

# Redis backup
docker compose exec redis redis-cli --raw SAVE
```

## Network

All services run on the `healthcare-network` bridge network, allowing them to communicate with each other using service names as hostnames.

## Environment Variables

Copy `.env.example` to `.env` and customize as needed:
```bash
cp .env.example .env
```

Then update `docker-compose.yml` to use environment variables if desired.
