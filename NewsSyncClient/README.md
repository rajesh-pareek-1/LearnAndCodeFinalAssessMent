
```markdown
# NewsSync Console Client

The **NewsSync Console Client** is a cross-platform `.NET 7` console application designed to provide a streamlined, secure, and personalized news-reading experience for authenticated users. Built with clean code principles and layered architecture, it interacts with the `NewsSync.API` backend to fetch, display, and manage news content while maintaining simplicity in user experience and robustness in design.

---

## Overview

This console application enables end-users to log in with their credentials and interact with a curated list of articles based on their preferences, behaviors, and roles. Users can browse articles, save them for later, report inappropriate content, and react to articles with likes/dislikes. The system is designed to ensure maintainability, modularity, and secure interaction with the backend.

---

## Key Features

### Authentication
- Login functionality with secure password masking (input hidden via `Console.ReadKey`)
- JWT token-based session handling
- Role-based authorization (supports `User` role)

### Article Interaction
- View articles filtered by:
  - Date range
  - Search query
  - User preferences (category-based ranking)
- Top 50 articles are prioritized and returned from the server
- Real-time interaction with NewsSync backend API

### Saved Articles
- Fetch and view user-saved articles
- Save new articles for future reading
- Remove saved articles

### Reactions
- Like or dislike articles
- View articles the user has reacted to
- Filter liked/disliked content

### Reporting
- Report inappropriate, misleading, or problematic articles
- Structured reporting sent to the backend for moderation

---

## Technical Stack

| Layer            | Technology Used                     |
|------------------|-------------------------------------|
| Presentation     | .NET 7 Console Application          |
| Services         | Custom DI-registered service layer  |
| Authentication   | JWT Token-based Authentication      |
| API Integration  | RESTful communication with ASP.NET Core backend |
| Architecture     | Layered and modular clean code structure |
| Logging & Errors | Custom Console Helpers + Exception Wrapping |

---

## Architecture and Structure

The project follows a **layered architecture**, strictly separating concerns across UI, Core logic, and Infrastructure communication.

```

NewsSyncClient/

.
├── Application
│   ├── Services
│   └── UseCases
├── appsettings.json
├── ConsoleAppHost.cs
├── Core
│   ├── Exception
│   ├── Interfaces
│   └── Models
├── Infrastructure
│   ├── Api
│   ├── Repositories
│   └── Security
├── NewsSyncClient.csproj
├── NewsSyncClient.sln
├── Presentation
│   ├── Helpers
│   ├── Prompts
│   ├── Renderers
│   └── Screens
├── Program.cs
└── README.md

```

---

## Clean Code Practices

The application adheres to the core principles of Clean Code by Robert C. Martin:

- **Single Responsibility Principle**: Each class/screen/service does one thing.
- **Descriptive Naming**: Classes, methods, and variables are named for clarity.
- **Minimal Duplication**: Reuse of helper methods and service interfaces.
- **Error Handling**: Structured exception handling with meaningful user messages.
- **Testability and Maintainability**: Clear separation of concerns to support unit testing and scalability.

---

## Security Considerations

- **Password Input Masking**: Users input passwords securely using hidden character inputs.
- **In-Memory Token Handling**: JWT tokens are managed only for the session lifecycle.
- **Error Abstraction**: User-friendly error messages; detailed logs for debugging.
- **Model Validation**: Backend enforces validation via `ValidateModelAttribute`.

---

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download)
- Running instance of [NewsSync.API](https://github.com/your-org/NewsSync.API) backend

### Running the App

```bash
dotnet run
```

On launch, the user is presented with an interactive login screen. Upon successful authentication, a role-based main menu is shown for article interaction.

---

## Sample User Journey

1. Launch console app.
2. Log in using your registered credentials.
3. Navigate through:
   * Browse filtered articles
   * Save favorite articles
   * React (like/dislike) to news
   * Report any problematic content
4. Logout securely when done.

---

## Future Improvements

* CLI support with flags for advanced users
* Integration with notification and background sync (for premium plans)

---

## Contributing

This project is under active development. Contributions aligned with clean code and modular architecture are welcome.

To contribute:

1. Fork the repository.
2. Create a new branch with your feature/fix.
3. Submit a pull request with proper documentation and rationale.

---

## Maintainers

The Rajesh Pareek - JSE at ITT maintains this project with a focus on software craftsmanship and usability. Please contact the him for major feature requests or design discussions.
