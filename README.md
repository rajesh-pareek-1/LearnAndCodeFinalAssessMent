### **Repository Description**

This Repository includes the final phase of the complete NewsSync Workshop project, combining both:

* `NewsSync.API` (Backend)
* `NewsSyncClient` (Console Application)

into the `main` branch for final review.

---

### Included Projects

#### NewsSync.API (Backend)

A modern, clean-architecture ASP.NET Core 9.0 backend for:

* User Authentication (JWT)
* Article Ingestion via external APIs (`NewsAPI.org`, `TheNewsAPI`)
* Role-based Access Control
* Article Reactions, Saved Articles, and Reporting
* Email Notifications via SMTP
* Admin APIs for content moderation

**Tech Highlights**:

* ASP.NET Core 9.0, EF Core, SQL Server (Docker), Serilog
* Clean separation of Domain, Application, Infrastructure, and API layers
* Unit testing with xUnit, Moq, FluentAssertions
* Logging, validation, exception handling best practices

---

#### NewsSyncClient (Console App)

A clean-code based .NET 9 console application for end users to:

* Log in and browse personalized news
* Save, like, dislike, and report articles
* Interact securely with NewsSync.API using JWT
* Experience a clean and modular UI flow in the terminal

**Highlights**:

* Secure password input
* Console rendering and interactive flows
* Role-based dynamic menus
* In-memory session handling

---

### Project Context

This repository and its sub-projects were developed as part of the **Learn and Code Program Workshop Assessment** at **In Time Tec Pvt. Ltd.**, with the goal of deeply understanding and implementing Clean Code practices from the book *"Clean Code"* by **Robert C. Martin**.

---

### Author & Maintainer

**Rajesh Pareek**
Junior Software Engineer (JSE)
In Time Tec Pvt. Ltd.

---

### Current Status

* Development complete
* Project is now closed for changes
* Open for review by mentors, leads, and contributors

