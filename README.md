# Ticket Booking
Netpower internship project

## Repository structure
```
| - - - ticket-booking-webapp/
|
|
| - - - ticket-booking-api/
			|
			| - - - TicketBooking.API/
```

## Installation and Run
### Setup local database instruction
1. Install Docker on Linux or WSL
2. Make sure you stop your local SQL Server database service
3. Running database using Docker
```bash
docker pull mcr.microsoft.com/azure-sql-edge
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=TicketBooking.database.v1' -p 1433:1433 --name azuresql -d mcr.microsoft.com/azure-sql-edge
```
4. Database is running now. If you want to start database service later , run
```bash
docker start azuresql
```

### Go to TicketBooking.API project in "ticket-booking-api"
1. Setup data
```bash
dotnet ef migrations add init
dotnet ef database update
```
2. Run API
```bash
dotnet run
```

### Go to folder "ticket-booking-webapp"
> run
```bash
npm i
npm start
```

> Now webapp is running at <<<http://127.0.0.1:3000>>> on your local machine.

## Tech stack for Ticket Booking

- Web application: ReactJS

- API: ASP.NET Core 7.0, Entity Framework Core 7.0

- Other services: Azure Database, Azure Blob Storage
