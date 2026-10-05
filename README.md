# SkillBridge

A candidate–job matching MVP: employers post jobs, candidates browse and apply, and employers review applicants.

## Repository layout

- `backend/SkillBridge.sln` — .NET solution containing the Onion Architecture projects (`TB.Api`, `TB.Application`, `TB.Domain`, `TB.Infrastructure`).
- `frontend/` — Angular standalone application.
- `frontend/public/wireframes/` — static wireframes for the six initial workflows.
- `plan.md` — scope, delivery rules, and acceptance criteria.

## Backend setup

Requirements: .NET 10 SDK, PostgreSQL, and the `dotnet-ef` 10 CLI tool.

1. Copy `example.env` to `.env` and set the PostgreSQL password in `ConnectionStrings__DefaultConnection`.
2. Restore and build the solution:

   ```sh
   dotnet restore backend/SkillBridge.sln
   dotnet build backend/SkillBridge.sln
   ```

3. Apply the initial schema and seed data from the repository root:

   ```sh
   dotnet ef database update --project backend/TB.Infrastructure --startup-project backend/TB.Api
   ```

   Or, from `backend/TB.Infrastructure`, run the short form:

   ```sh
   dotnet ef database update
   ```

4. Start the API:

   ```sh
   dotnet run --project backend/TB.Api
   ```

The API exposes `GET /health`. The EF Core model has six tables: `users`, `skills`, `candidate_skills`, `jobs`, `job_skills`, and `applications`. Roles and application statuses are represented by domain enums and stored as readable strings. Applications have a unique job/candidate constraint and only allow the `Received` → `Shortlisted` or `Rejected` transitions. The initial migration seeds one employer, one candidate, eight skills, three jobs, and the corresponding candidate/job skill links. Demo user rows deliberately have a disabled seed-only password marker; authentication is not implemented by this phase.

To regenerate the migration after an intentional model change:

```sh
dotnet ef migrations add <MigrationName> --project backend/TB.Infrastructure --startup-project backend/TB.Api
```

## Frontend setup

Requirements: Node.js and npm.

```sh
cd frontend
npm install
npm start
```

View the six static screen wireframes at `/wireframes/index.html` from the Angular development server.

## Team role assignments

The team roster was not provided, so assignments are by role; map a teammate to each role before implementation begins.

| Team role | Primary ownership | Shared handoff |
|---|---|---|
| Lead / Pitch | Scope, priorities, demo flow, and acceptance tracking | Keeps the team aligned on the job-post → browse → apply → review journey |
| Frontend | Angular routing, Login/Register/Jobs/My Applications/Create Job/Applicants screens, forms, and UI states | Integrates against backend DTOs and reports contract gaps |
| Backend | ASP.NET API controllers, services, validation, authorization, and response DTOs | Coordinates endpoint contracts with Frontend |
| Database / Full-stack | Entities, EF Core mapping, migrations, constraints, and seed data | Reviews query shape and data integrity with Backend |
| QA / Integrator | Build/run checks, end-to-end scenarios, README, and demo readiness | Verifies acceptance criteria and catches regressions across both projects |

For a smaller team, Backend and Database / Full-stack may be assigned to one person; QA / Integrator remains a shared responsibility.

## Current scope boundaries

Phase 1 establishes the projects, domain entities, initial schema, deterministic sample data, and wireframes. Authentication, job/application endpoints, server-side match calculation, real login credentials, and production deployment are follow-up implementation work.
