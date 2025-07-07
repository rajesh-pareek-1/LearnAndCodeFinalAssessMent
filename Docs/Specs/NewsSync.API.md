NewsSync.API

`NewsSync.API` is the backend server for the NewsSync platform — a modern news aggregation and personalization system. It handles user authentication, article ingestion, notifications, saved articles, and admin operations. Built using  **ASP.NET Core** ,  **EF Core** , and follows clean architecture principles.

---

### Project Structure

```
NewsSync.API/
├── API/                     # Controllers, Middleware, Filters
├── Application/            # DTOs, Services, Interfaces, Exceptions
├── Domain/                 # Entities, Constants, Validation Messages
├── Infrastructure/         # DB Contexts, Repositories, DI, Adapters
├── Logs/                   # Rolling application logs
├── appsettings*.json       # Configuration files
├── Program.cs              # Entry point
```

---

### Tech Stack

| Area              | Technology                    |
| ----------------- | ----------------------------- |
| Framework         | ASP.NET Core 9.0              |
| ORM               | Entity Framework Core         |
| Database          | SQL Server (via Docker)       |
| Authentication    | JWT                           |
| Background Jobs   | Hosted Services               |
| DI Container      | Built-in Dependency Injection |
| Logging           | Serilog (or .NET Logging)     |
| Testing Framework | xUnit, Moq, FluentAssertions  |

---

### Getting Started

#### Prerequisites

* [.NET 9 SDK](https://dotnet.microsoft.com/)
* Docker (for SQL Server)
* Azure Data Studio / SSMS (optional)

#### 1. Clone the Repo

```bash
git clone https://github.com/rajesh-pareek-1/LearnAndCodeFinalAssessMent.gitcd 
```

#### 2. Set Up the Database in Mac OS

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" \
-p 1433:1433 --name sqlserver \
-d mcr.microsoft.com/mssql/server:2022-latest
```

Update `appsettings.json` connection strings if needed.

#### 3. Apply Migrations (if applicable)

```bash
dotnet ef database update --project Infrastructure --startup-project NewsSync.API
```

#### 4. Run the API

```bash
dotnet run --project NewsSync.API
```

API will be available at `https://localhost:7202`

---

### Unit Tests

Tests are located in:

```
NewsSync.API.Tests/
├── Services/
│   ├── ArticleServiceTests.cs
│   ├── AdminServiceTests.cs
│   ├── NotificationServiceTests.cs
│   ├── SavedArticleServiceTests.cs
├── Adapters/
│   ├── NewsApiOrgClientAdapterTests.cs
│   ├── TheNewsApiClientAdapterTests.cs
```

Run tests using:

```bash
dotnet test NewsSync.API.Tests
```

---

### Security

* JWT authentication via `Bearer` tokens
* Secrets (API keys, SMTP credentials) are not committed
* `appsettings.json` should be secured via environment-specific configuration

---

### Email Notifications

Configured via Gmail SMTP. In production, use environment secrets:

```json
"Email": {
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "Username": "<your-email>",
    "Password": "<your-app-password>",
    "From": "noreply@yourdomain.com"
  }
}
```

---

### Core Features

* User Registration & Login (JWT-based)
* Article Fetching via Adapters (`NewsAPI.org`, `TheNewsAPI`)
* Save Articles for Later
* Report and Auto-block Offensive Articles
* Email-based Notifications with Preferences
* Admin APIs to manage categories and block content

---

### Clean Code Practices

* Services are testable and follow the Single Responsibility Principle
* Repositories abstract EF Core logic
* DTOs enforce separation from domain models
* Controllers are thin; business logic resides in Services
* Logs are context-rich and structured

---

### Contributing

We welcome PRs that improve testability, add missing test coverage, or introduce new providers/adapters
