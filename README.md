# Restaurant Table Reservation System

Layered ASP.NET Core Web API for managing restaurant reservation slots and table reservations. Uses MongoDB, Serilog-friendly logging, Swagger, health checks, Mapster, FluentValidation, and xUnit + Moq tests. Docker compose spins up API + Mongo.

## Project Layout

```
restaurant_table_reservation_system.sln
Dockerfile
docker-compose.yml
src/
  RestaurantReservation.Host/        # ASP.NET Core host (controllers, DI, options)
  RestaurantReservation.BL/          # Business layer (services, DTOs, options)
  RestaurantReservation.DAL/         # Data layer (Mongo context + repositories)
tests/
  RestaurantReservation.Tests/       # xUnit + Moq tests for business logic
```

## Running locally

1) Start MongoDB (or rely on docker-compose):
```bash
docker run -d --name mongo -p 27017:27017 mongo:7
```
2) Run the API:
```bash
dotnet run --project src/RestaurantReservation.Host
```
Swagger: http://localhost:5127/swagger
Health: http://localhost:5127/health

## Docker Compose
```bash
docker-compose up --build
```
Exposes API on port 5127 and MongoDB on 27017.

## Configuration
App settings (env vars supported):
- `Mongo:ConnectionString`
- `Mongo:DatabaseName`
- `Reservation:MaxSeatsPerUser`
- `Reservation:ReservationFeePercent` (decimal fraction)
- `Reservation:AllowReservationAfterStart`

## API Surface
- `GET  /api/ping`
- `GET  /health`
- `GET  /api/slots`
- `GET  /api/slots/{id}`
- `POST /api/slots`
- `PUT  /api/slots/{id}`
- `DELETE /api/slots/{id}`
- `POST /api/reservations/reserve` with body `{ userId, slotId, quantity }`

## Tests
```bash
dotnet test
```
Covers reservation happy-path and insufficient-spots cases.
