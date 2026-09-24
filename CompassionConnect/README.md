<<<<<<< HEAD
﻿# Gift of the Givers â€” ASP.NET Core MVC (.NET 8)
=======
# Gift of the Givers — ASP.NET Core MVC (.NET 8)
>>>>>>> origin/main

Gift of the Givers Foundation Disaster Relief Management System, converted from the original React + TypeScript + Vite application into a production-shaped ASP.NET Core MVC (.NET 8) solution using C#, Entity Framework Core, SQL Server and ASP.NET Identity.

## Solution layout

<<<<<<< HEAD
```
=======
```text
>>>>>>> origin/main
GiftOfTheGivers.sln
GiftOfTheGivers.Web/
  Controllers/        Home, Account, Donation, Volunteer, Project, Employee, News, Donor
  Models/              Employee, Donor, ApplicationUser, ReliefProject, Donation,
                       TaxCertificate, Volunteer, NewsArticle
  ViewModels/          Donate, VolunteerRegister, ProjectEdit, Account*, Dashboard*
  Data/                ApplicationDbContext (EF Core + Identity), SeedData
  Repositories/        Generic IRepository<T>/Repository<T> + entity repos + UnitOfWork
  Services/            DonationService, VolunteerService, ProjectService,
                       TaxCertificateService (PDF via QuestPDF), ReferenceNumberService,
                       NewsService
  Views/               Razor views for every controller action, Shared/_Layout.cshtml
  wwwroot/             site.css (brand theme), site.js, placeholder campaign/news images
```

## Architecture

<<<<<<< HEAD
- **Repository pattern** â€” a generic `Repository<T>` handles common CRUD; entity-specific
  repositories (`DonationRepository`, `VolunteerRepository`, `ProjectRepository`, etc.)
  add targeted queries. All repositories are coordinated through a single `UnitOfWork`
  so each request commits one `SaveChanges` transaction.
- **Service layer** â€” business rules (resolving/creating a donor, generating unique
  reference numbers, issuing tax certificates, updating project totals) live in
  `Services/*Service.cs`, injected into controllers via interfaces.
- **Dependency Injection** â€” everything (`DbContext`, Identity, repositories, services)
  is registered in `Program.cs`.
- **ViewModels** â€” controllers never pass EF entities directly into forms; `ViewModels/`
  hold validation attributes and shape data for each view.
=======
* **Repository pattern** — a generic `Repository<T>` handles common CRUD; entity-specific repositories (`DonationRepository`, `VolunteerRepository`, `ProjectRepository`, etc.) add targeted queries. All repositories are coordinated through a single `UnitOfWork` so each request commits one `SaveChanges` transaction.
* **Service layer** — business rules (resolving/creating a donor, generating unique reference numbers, issuing tax certificates, updating project totals) live in `Services/*Service.cs`, injected into controllers via interfaces.
* **Dependency Injection** — everything (`DbContext`, Identity, repositories, services) is registered in `Program.cs`.
* **ViewModels** — controllers never pass EF entities directly into forms; `ViewModels/` hold validation attributes and shape data for each view.
>>>>>>> origin/main

## Authentication & roles

ASP.NET Identity is configured with two roles: **Employee** and **Donor**.

<<<<<<< HEAD
- `AccountController` exposes separate `LoginDonor` / `LoginEmployee` actions (matching
  the original app's distinct donor vs staff sign-in), plus `Register`, `Profile`,
  `Settings` (change password) and `ForgotPassword`.
- **Anonymous donations are fully supported** â€” `DonationController.Index` works with or
  without an authenticated user; a guest donor record is created automatically from the
  form's name/email so a tax certificate can still be issued.
- Demo accounts are seeded automatically on first run (see below).
=======
* `AccountController` exposes separate `LoginDonor` / `LoginEmployee` actions (matching the original app's distinct donor vs staff sign-in), plus `Register`, `Profile`, `Settings` (change password) and `ForgotPassword`.
* **Anonymous donations are fully supported** — `DonationController.Index` works with or without an authenticated user; a guest donor record is created automatically from the form's name/email so a tax certificate can still be issued.
* Demo accounts are seeded automatically on first run (see below).
>>>>>>> origin/main

## Database

EF Core Code-First against SQL Server (LocalDB by default, Azure SQL compatible via the connection string in `appsettings.json`). `SeedData.cs` runs on startup and:

1. Applies pending migrations (`context.Database.MigrateAsync()`).
2. Creates the `Employee` and `Donor` Identity roles.
3. Seeds 6 sample relief projects and 3 news articles.
4. Creates one demo Employee and one demo Donor account.

### Demo accounts

| Role     | Email                                                               | Password       |
| -------- | ------------------------------------------------------------------- | -------------- |
| Employee | [employee@giftofthegivers.org](mailto:employee@giftofthegivers.org) | Employee@2026! |
| Donor    | [donor@example.com](mailto:donor@example.com)                       | Donor@2026!    |

## Reference numbers

`ReferenceNumberService` generates unique, sequential, human-readable references per calendar year:

* Donations: `DON-2026-000001`
* Volunteer applications: `VOL-2026-000001`
* Section 18A tax certificates: `GG18A-2026-000001`

## Tax certificates

Every completed donation automatically receives a Section 18A tax certificate (`TaxCertificateService.IssueForDonationAsync`). Certificates can be viewed from the donor dashboard and downloaded as a printable PDF generated server-side with [QuestPDF](https://www.questpdf.com/) under its Community license via `DonorController.DownloadCertificate`.

## Running the project (Visual Studio 2022)

<<<<<<< HEAD
1. Open `GiftOfTheGivers.sln` in Visual Studio 2022 (17.8+) with the **ASP.NET and web
   development** workload installed.
2. Restore NuGet packages (VS does this automatically on open, or `dotnet restore`).
3. Update the `DefaultConnection` string in `appsettings.json` if you're not using
   LocalDB.
=======
1. Open `GiftOfTheGivers.sln` in Visual Studio 2022 (17.8+) with the **ASP.NET and web development** workload installed.

2. Restore NuGet packages. Visual Studio does this automatically on open, or run `dotnet restore`.

3. Update the `DefaultConnection` string in `appsettings.json` if you're not using LocalDB.

>>>>>>> origin/main
4. In **Package Manager Console**, run:

   ```text
   Add-Migration InitialCreate -Project GiftOfTheGivers.Web
   Update-Database -Project GiftOfTheGivers.Web
   ```
<<<<<<< HEAD
   Add-Migration InitialCreate -Project GiftOfTheGivers.Web
   Update-Database -Project GiftOfTheGivers.Web
   ```
   (Or from the CLI: `dotnet ef migrations add InitialCreate` then `dotnet ef database update`,
   run from the `GiftOfTheGivers.Web` folder.)
5. Press **F5** / Start. The app seeds the database automatically on first launch.

## Azure deployment notes

- The connection string and app settings are already externalised in
  `appsettings.json` / `appsettings.Development.json` â€” replace with an Azure SQL
  connection string (and ideally move secrets to Azure App Service Configuration or
  Key Vault) for production.
- `sql.EnableRetryOnFailure()` is enabled on the `DbContext` for Azure SQL transient
  fault tolerance.
- The project targets `net8.0` and uses only cross-platform packages, so it is ready
  for Azure App Service (Windows or Linux) or containerised deployment.

## Known follow-ups

This conversion was produced without a live .NET SDK/compiler in the authoring
environment, so while the code was written carefully against .NET 8 / EF Core 8 /
ASP.NET Identity APIs, you should do a first `dotnet build` pass in Visual Studio and
fix any residual compiler errors before deploying. Likely areas to double check:
- EF Core migration generation (enum-as-string conversions, decimal precision).
- Minor Razor syntax edge cases in conditional `class`/`selected` attributes.
- QuestPDF Community license activation (already set in `TaxCertificateService`).

=======

   Or from the CLI:

   ```text
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

   Run the CLI commands from the `GiftOfTheGivers.Web` folder.

5. Press **F5** / Start. The application seeds the database automatically on first launch.

## Azure deployment notes

* The connection string and app settings are externalised in `appsettings.json` / `appsettings.Development.json`. Replace the connection string with an Azure SQL connection string for production and ideally move secrets to Azure App Service Configuration or Azure Key Vault.
* `sql.EnableRetryOnFailure()` is enabled on the `DbContext` for Azure SQL transient fault tolerance.
* The project targets `net8.0` and uses cross-platform packages, making it suitable for Azure App Service (Windows or Linux) or containerised deployment.

## Known follow-ups

This conversion was produced without a live .NET SDK/compiler in the authoring environment. The code should therefore be validated with a first `dotnet build` pass in Visual Studio before deployment.

Potential areas to double-check:

* EF Core migration generation (enum-as-string conversions and decimal precision).
* Minor Razor syntax edge cases in conditional `class`/`selected` attributes.
* QuestPDF Community license activation, which is configured in `TaxCertificateService`.
>>>>>>> origin/main
