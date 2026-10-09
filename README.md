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

The API exposes `GET /health`. The EF Core model has six tables: `users`, `skills`, `candidate_skills`, `jobs`, `job_skills`, and `applications`. Roles and application statuses are represented by domain enums and stored as readable strings. Applications have a unique job/candidate constraint and only allow the `Received` → `Shortlisted` or `Rejected` transitions. The migrations seed one employer, one candidate, eight skills, six jobs, their skill links, and two candidate applications (one `Received`, one `Shortlisted`). Demo accounts both use the password `Password123!`.

### Demo accounts

| Role | Email | Password |
|---|---|---|
| Candidate | `candidate@skillbridge.demo` | `Password123!` |
| Employer | `employer@skillbridge.demo` | `Password123!` |

### API endpoints

| Method | Route | Access | Purpose |
|---|---|---|---|
| `GET` | `/health` | Anonymous | Liveness probe |
| `POST` | `/api/auth/register` | Anonymous | Register a candidate or employer, returns a JWT |
| `POST` | `/api/auth/login` | Anonymous | Authenticate and return a JWT |
| `GET` | `/api/auth/me` | Authenticated | Current user |
| `GET` | `/api/skills` | Anonymous | List all skills |
| `GET` | `/api/jobs` | Anonymous | Browse jobs (optional `?skillId=` filters) |
| `GET` | `/api/jobs/{id}` | Anonymous | Job detail |
| `POST` | `/api/jobs` | Employer | Create a job |
| `PUT` | `/api/jobs/{id}` | Employer | Update an owned job |
| `DELETE` | `/api/jobs/{id}` | Employer | Delete an owned job |
| `POST` | `/api/jobs/{id}/applications` | Candidate | Apply once to a job |
| `GET` | `/api/employer/jobs` | Employer | List the current employer's jobs |
| `GET` | `/api/employer/jobs/{id}/applicants` | Employer | Applicants with server-calculated match % |
| `PATCH` | `/api/applications/{id}/status` | Employer | Set `Shortlisted` / `Rejected` |
| `GET` | `/api/candidates/me` | Candidate | Current candidate profile |
| `PUT` | `/api/candidates/me` | Candidate | Update profile and skills |
| `GET` | `/api/candidates/me/applications` | Candidate | The current candidate's applications |
| `POST` | `/api/candidates/me/cv` | Candidate | Upload a PDF CV to import skills |

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

## Frontend overview

- Angular 22 standalone components with lazy `loadComponent` routes and reactive forms.
- JWT session handling: the token, and the signed-in user (including role), are persisted by `AuthService`; `roleGuard` protects the candidate and employer areas.
- Screens: landing, role select, login, candidate/employer registration, candidate dashboard (browse + apply), candidate applications, candidate profile edit, CV upload, employer dashboard (job management), job create/edit, and job applicants (match % + status updates).
- View the six low-fidelity screen wireframes at `/wireframes/index.html` from the Angular development server.

## Current status

The primary journey is implemented end to end and verified: a candidate registers, updates skills (manually or from a CV), browses and applies once to a job; an employer posts a job with required skills and reviews applicants with a server-calculated match percentage, then sets an application to `Shortlisted` or `Rejected`. Unfinished items (saved jobs, notifications, recruiter talent search, file attachments beyond CV import, and production deployment) remain out of scope per `plan.md`.

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
