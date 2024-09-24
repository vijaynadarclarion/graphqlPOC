# NAJM ASP.NET Core Reference Application / Solution Template

Sample ASP.NET Core reference application, based on clean (onion) architecture and DDD, demonstrating a single-process (monolithic) application architecture and deployment model. 

## Clean Architecture Layers

### Domain Layer

In Ideal DDD implementation this Layer would contain Entites, Aggregate Roots, Value Objects, Domain Services, and Specifications
But in our implementation it is simplified for now to contain just entities generated from EF scaffolding in DB First approach

Contains:
- Domain Entities
- Infrastructure contracts / Interfaces (repositories, unit of work, proxies)
- Integration DTOs

## Application Core Layer

Contains:
- DTOs: Data Transfer Objects used in communication between Presentation Layer and Application Core Layer
- Business Validators: Custom Business Validators for Application services business.
- Specifications: encapsulating query details
- Object Mappers: Define mappers between DTOs and Business Entities
- Application Services: 
  Implement the use cases of an application. used by presentation layer
  Application services use Repositories, Unit Of Work, Mappers, Validators, and Infrastructure Services
  Queries are created as specifications objects and passed to repositories
- Background Tasks:
  Use Application Services for executing scheduled and long running background tasks 

### Infastructure Layer

Depends on:
- Domain Layer

Contains:
- Persistence (database) implementation
- Integration Proxies implementation for integrations
- Supportive External Services (email service, sms service, ....)

## Presentation Layer

 Depends on
 - Application Core Layer
 - Domain Layer
 - Infrastructure Layer

 Contains:
 - API controllers or Razor Pages: using application services
 - IoC Container Configurations: configure services implementations, and infrastructure
 - Error Handling: global exception handling middleware

## Design Principles Applied
 - Fail Fast
 - SOLID
 - https://deviq.com/principles/principles-overview

## DDD Concepts

Ubiquitous Language, Subdomains, Specification, Repository

## Design Patterns Applied

 - Dependancy Injection: loose coupling system components.
 - Repository: abstracting persistence details (database, files, ...).
 - Proxy: abstracting external integrations like external APIs.
 - Factory: encapsulating complex object creation.
 - Services: encapsulating complex behavior and/or infrastructure implementation details
 - Specifications: encapsulating query details
 ---- [Ongoing] UnitOfWork: abstraction and control on a database connection and transaction scope in an application.
 - Options pattern: uses classes to provide strongly typed access to groups of related settings
 - Guard clause: simplifies complex functions by "failing fast", checking for invalid inputs up front and immediately failing if any are found.
 - Resilience design patterns: retry, timeout, circuit breaker
 - Rules Engine: simplfy complext conditional besinesses with business rules processing the same entity.
 - Anti-Corruption Layer (ACL): A set of patterns placed between the domain model and other bounded contexts or third party dependencies. The intent of this layer is to prevent the intrusion of foreign concepts and models into the domain model. 
   This layer is typically made up of several well-known design patterns such as Facade and Adapter. 
   The patterns are used to map between foreign domain models and APIs to types and interfaces that are part of the domain model.

## Adf Services and application modules

Has Contracts and implementation for crosscutting concerns and commonly needed base functionality in most of applications

 - identity (authentication / authorization)
 - Data Protection for safely storing senstive data
 - caching (server side distributed caching, Per request cache, output caching, ...)
 - error handling (global exception handling, defining custom exceptions for max control on error handling)
 - logging (file system and database using serlilog + using correlation id)
 - localizaion, and globalization (cookie based localization, culture management, datetime calendars, )
 - configuration and settings management (in sql server database)
 - auditing (api calls audit, data changes audit)
 - background tasks (using hangfire .. [considering change])
 - notifications (mobile, web sockets push, emails, SMS, ...)
 - middlewares for pre and after request/response processing piplelines (audit api calls, security headers, correlation Id)
 ---- [Ongoing] security (security headers, encryption, working with certificates, data protection ...)
 ---- [Ongoing] reporting (create pdf, word, csv, and Excel)

## TDD and Unit testing
 - Unit tests is a neccessaty the for success of critical complex projects that has many moving parts


 references for dev team
 https://www.infoq.com/minibooks/domain-driven-design-quickly/
 https://dzone.com/refcardz/getting-started-domain-driven

## EF Commands

DotNet Tool for scaffolding .. recommended over powershell approach
dotnet tool install --global dotnet-ef --version 5.0.14

------------------------------- Scaffold database ------------------------------------------------------------

Scaffold Entities to ApplicationCore Layer and DbContext to Infrastructure Layer
cd .\src\NUP.Infrastructure\
dotnet ef dbcontext scaffold "Server=NJDRIY-007\ONADASQL1;Database=NajmNetReplica;Trusted_Connection=True;" --schema "moh" Microsoft.EntityFrameworkCore.SqlServer --context "AppDbContext" --context-namespace "NUP.Infrastructure.Data" --context-dir "Data" --namespace "NUP.ApplicationCore.Entities" --output-dir "..\NUP.ApplicationCore\_Entities"
dotnet ef dbcontext scaffold "Server=NJDRIY-007\ONADASQL1;Database=NajmNetReplica;Trusted_Connection=True;" --schema "moh" Microsoft.EntityFrameworkCore.SqlServer --context "AppReadOnlyDbContext" --context-namespace "NUP.Infrastructure.Data" --context-dir "Data" --namespace "NUP.ApplicationCore.Entities" --output-dir "..\NUP.ApplicationCore\_Entities"

-------------------------------- Code First Commands ---------------------------------------------------------

-- Make Web Project as satrtup prohect >> that has startupfile configured and connections strings added

-- Change default project to Adf.Identity
Add-Migration -Context AppIdentityDbContext "Initial_identity" -o ./_Infrastructure/Data/IdentityMigrations
Update-Database  -Context AppIdentityDbContext

Add-Migration -Context DataProtectionDbContext "initial_data_keys" -o ./_Infrastructure/Data/DataKeysMigrations
Update-Database  -Context DataKeysContext

-- Change default project to Adf.CommonServices
Add-Migration -Context CommonsDbContext "Initial_commons" -o ._Infrastructure/Data/Migrations
Update-Database  -Context CommonsDbContext

-- dont use the following commands if you are using database first (scaffolding)
-- Change default project to NUP.Infrastructure
Add-Migration -Context AppDbContext "Initial_appdb" -o ./Data/Migrations
Update-Database -Context AppDbContext

-----------------------------------------------------------------------------------------------------------------------