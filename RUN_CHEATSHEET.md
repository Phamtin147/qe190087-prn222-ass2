# FUNewsManagementSystem Assignment 02 Run Cheatsheet

## 1. Requirements

- .NET SDK for this project: `net10.0`
- SQL Server running through Docker:
  - Host: `localhost` / `127.0.0.1`
  - Port: `14330`
  - User: `sa`
  - Password: `123`

Check SQL Server:

```bash
docker ps --filter name=sqlserver-lab
tsql -H localhost -p 14330 -U sa -P '123'
```

The Docker mapping is `14330 -> 1433`, so use port `14330` from the host and from DBeaver.

## 2. Database

The app is configured to use:

```text
FUNewsManagement_Assignment02
```

On startup, EF Core creates the database schema with `Database.EnsureCreated()` and seeds demo accounts from `Program.cs`.

Connection string in `FUNewsManagementSystem.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "FUNewsManagement": "Server=localhost,14330;Database=FUNewsManagement_Assignment02;User Id=sa;Password=123;TrustServerCertificate=True"
}
```

## 3. DBeaver

Create an `MS SQL Server` connection:

- Host: `127.0.0.1`
- Port: `14330`
- Database: `master` for first test, or `FUNewsManagement_Assignment02` after the app has started once
- Username: `sa`
- Password: `123`

Driver properties:

- `encrypt=false`
- `trustServerCertificate=true`

## 4. Restore and build

```bash
dotnet restore FUNewsManagementSystem.slnx
dotnet build FUNewsManagementSystem.slnx
```

If there is no solution file in this folder, build the web project directly:

```bash
dotnet build FUNewsManagementSystem.Web/FUNewsManagementSystem.Web.csproj
```

Expected result:

```text
Build succeeded.
0 Error(s)
```

## 5. Run web app

```bash
dotnet run --project FUNewsManagementSystem.Web/FUNewsManagementSystem.Web.csproj
```

Or force a port:

```bash
dotnet run --project FUNewsManagementSystem.Web/FUNewsManagementSystem.Web.csproj --urls http://localhost:5098
```

Open:

```text
http://localhost:5098
```

## 6. Demo accounts

Admin account from `appsettings.json` and seed data:

```text
Email: admin@FUNewsManagementSystem.org
Password: @@abc123@@
```

Seeded demo users:

```text
Email: staff@funews.org
Password: 123

Email: lecturer@funews.org
Password: 123
```

## 7. Quick test flow

1. Start SQL Server Docker container.
2. Build the project.
3. Run the web app.
4. Login as admin or staff.
5. View news list.
6. Use staff pages to create/edit news and categories.
7. Confirm SignalR updates are available through `/newsHub`.
8. Use admin pages to view accounts/report.
