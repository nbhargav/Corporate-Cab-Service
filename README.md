# Corporate Cab Service - Vehicle & Fleet Management System

A centralized, role-based web application that replaces manual, phone-driven coordination
of company vehicles, drivers, and employee transport requests.

## Roles & workflow

- **Employee** — submits a vehicle request (purpose, time window, pickup/drop location),
  tracks its status, and leaves feedback after a trip.
- **Admin** — reviews pending requests, assigns an available vehicle (and driver), rejects
  requests, and views fleet-wide fuel and feedback reports.
- **Driver** — views assigned vehicles and logs fuel usage (quantity, cost, odometer reading).

```
Employee submits request -> Admin reviews & assigns vehicle -> Driver logs fuel ->
Admin views reports / employee feedback
```

## Architecture

- **ASP.NET Core MVC** (C#) — controllers per role/domain area (`AccountController`,
  `RequestController`, `AdminController`, `DriverController`)
- **Entity Framework Core** over **SQL Server** — code-first models with relationships
  (`Employee` 1—N `VehicleRequest`, `VehicleRequest` 1—1 `AssignedVehicle`, `Vehicle` 1—N
  `FuelLog`/`Feedback`, `Vehicle` 1—1 `VehicleInsurance`)
- **Session-based auth** with a `RequireRoleAttribute` action filter that gates each
  controller to its role, rather than duplicating role checks in every action
- **BCrypt** password hashing (no plaintext/reversible passwords)

This uses the modern ASP.NET Core MVC + EF Core stack rather than the legacy Web Forms /
raw ADO.NET approach in the original project report, since it's the actively maintained,
still-relevant version of the same skill set (C#, SQL Server, MVC architecture, 3-tier
separation of concerns) for a portfolio audience.

## Project structure

```
CabService/
  Models/       - Employee, Vehicle, VehicleRequest, AssignedVehicle, FuelLog, Feedback, VehicleInsurance
  Data/         - CabServiceContext (EF Core DbContext)
  Controllers/  - AccountController, RequestController, AdminController, DriverController
  Filters/      - RequireRoleAttribute (role-based access control)
  Views/        - Razor views (Login, Admin Dashboard, etc.)
sql/schema.sql  - reference schema (mirrors the EF Core model)
```

## Running locally

1. Install the .NET 8 SDK and SQL Server (or SQL Server Express / LocalDB).
2. Update the connection string in `appsettings.json` if needed.
3. From `CabService/`:
   ```
   dotnet restore
   dotnet ef database update   # applies migrations (run `dotnet ef migrations add Initial` first)
   dotnet run
   ```
4. Visit `https://localhost:5001/Account/Register` to create your first user, then log in.

## Notes on scope

This is a from-scratch reference implementation of the system described in the project
report (role-based fleet management with request/assignment/fuel-log/feedback workflow),
rewritten on a current, runnable stack - not a transcription of legacy Web Forms code.
Some peripheral features from the original spec (e.g. detailed DFDs/UML docs, full
regression test suite) are summarized in the report rather than reproduced as code here.
