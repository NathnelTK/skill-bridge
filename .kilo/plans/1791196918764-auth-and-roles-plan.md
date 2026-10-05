# SkillBridge MVP — PRs #1a (Backend Auth) & #1b (Frontend Auth)

## Context

- Repo: `https://github.com/NathnelTK/skill-bridge` (branch `main`, current HEAD `1a6c7bf`).
- Backend: .NET 10 onion solution `backend/SkillBridge.sln` (`TB.Domain`, `TB.Application`, `TB.Infrastructure`, `TB.Api`). EF Core 10 + Npgsql/PostgreSQL (Supabase). Six tables already migrated: `users`, `skills`, `candidate_skills`, `jobs`, `job_skills`, `applications`.
- `User` already has `Email`, `PasswordHash`, `Role` (`Candidate`/`Employer`), `FullName`, `CompanyName?`, `Location?`, `CreatedAtUtc`. Unique email index exists. No schema change is required for auth.
- Seed users use the placeholder hash `!seed-only-account!`, so demo logins do not work yet.
- Frontend: Angular 22 standalone app (`frontend/`), currently the default placeholder with an empty `routes` array. No HTTP client, guards, or auth wiring yet.
- `.env` (gitignored) currently holds only `ConnectionStrings__DefaultConnection`. `example.env` is tracked.
- Note: the two PDF guides (`Building Productioncode.pdf`, `Enterprise Angular 22.pdf`) could not be parsed here (no PDF support). The plan follows the existing onion layout and standard ASP.NET Core + Angular 22 conventions.

## Decisions (confirmed)

- **PR strategy:** this feature ships as **two PRs**, not one, so the whole team can participate.
  - **PR #1a — Backend auth** (branch `feat/auth-backend` → `main`).
  - **PR #1b — Frontend auth** (branch `feat/auth-frontend` → `main`), based on `main` **after** #1a merges (the API contract freezes with #1a).
- **Auth:** custom JWT + BCrypt, **access token only** (no refresh tokens, no ASP.NET Core Identity).
- **JWT key handling:** a real 32+ char key is generated and written to the gitignored `.env` only; `example.env` gets a placeholder. No secret is committed.
- **Intentional team hand-off:** automated tests and the real feature screens are deliberately left unimplemented for teammates (see TEAM TASKS in each PR). The plan author implements features only; no test projects or specs are authored.

## Out of scope for both PRs (team backlog)

- Real feature screens for `/jobs`, `/create-job`, `/applicants`, `/my-applications` (roadmap PRs #2–#7).
- Backend unit/integration tests and frontend vitest specs.
- Refresh tokens, password reset, email verification, account lockout.

---

# PR #1a — Backend Auth (`feat/auth-backend`)

Registration/login/me endpoints with password hashing, JWT issuance, role-based authorization, exception handling, and CORS. No frontend changes in this PR.

## Tasks (ordered)

1. **Packages** — add to `TB.Infrastructure/TB.Infrastructure.csproj`:
   - `BCrypt.Net-Next`
   - `Microsoft.AspNetCore.Authentication.JwtBearer` (align to the `10.0.0` line used by EF).
   `TB.Application` stays package-free (references `TB.Domain` only).

2. **Application abstractions** (`TB.Application/Abstractions/`):
   - `IPasswordHasher` → `string Hash(string password)`, `bool Verify(string password, string hash)`.
   - `IJwtTokenGenerator` → `(string Token, DateTime ExpiresAtUtc) Generate(User user)`.
   - `IUserRepository` → `Task<User?> GetByEmailAsync(string email, CancellationToken ct)`, `Task<User?> GetByIdAsync(Guid id, CancellationToken ct)`, `Task<bool> EmailExistsAsync(string email, CancellationToken ct)`, `Task AddAsync(User user, CancellationToken ct)`.
   - `IAuthService` → `Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)`, `Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)`, `Task<UserDto> GetCurrentAsync(Guid userId, CancellationToken ct)`.

3. **Application DTOs** (`TB.Application/Auth/`):
   - `RegisterRequest`: `FullName`, `Email`, `Password`, `UserRole Role`, `CompanyName?`, `Location?` with DataAnnotations (`[Required]`, `[EmailAddress]`, `[StringLength]` matching schema caps: name 160, email 320, company 160, location 160, password min 8). Implement `IValidatableObject` to require `CompanyName` when `Role == Employer`.
   - `LoginRequest`: `Email` (`[Required][EmailAddress]`), `Password` (`[Required]`).
   - `AuthResponse`: `Token`, `ExpiresAtUtc`, `User` (`UserDto`).
   - `UserDto`: `Id`, `FullName`, `Email`, `Role`, `CompanyName`, `Location`.

4. **Application common exceptions** (`TB.Application/Common/Exceptions/`): `ValidationException`, `ConflictException`, `UnauthorizedException`, `ForbiddenException`, `NotFoundException` (each maps to 400/409/401/403/404).

5. **Application service** `TB.Application/Auth/AuthService.cs`:
   - `RegisterAsync`: trim + `ToLowerInvariant()` email; reject undefined `Role`; require company name for employers; `EmailExistsAsync` → `ConflictException`; hash password; create `User` with `Guid.NewGuid()` and `CreatedAtUtc = DateTime.UtcNow`; `AddAsync`; generate token; return `AuthResponse`.
   - `LoginAsync`: `GetByEmailAsync`; missing user or failed `Verify` → `UnauthorizedException`. Reject the legacy `!seed-only-account!` marker as invalid. Return token + user.
   - `GetCurrentAsync`: `GetByIdAsync` → `NotFoundException` if absent; map to `UserDto`.

6. **Infrastructure implementations**:
   - `TB.Infrastructure/Auth/BCryptPasswordHasher.cs` (BCrypt work factor 12).
   - `TB.Infrastructure/Auth/JwtTokenGenerator.cs` + `JwtOptions` (`Key`, `Issuer`, `Audience`, `ExpiryHours` default 8). Claims with default mapping: `ClaimTypes.NameIdentifier` (user id), `ClaimTypes.Email`, `ClaimTypes.Name`, `ClaimTypes.Role` (`Candidate`/`Employer`). Sign HS256.
   - `TB.Infrastructure/Persistence/Repositories/UserRepository.cs` using `AppDbContext` (`AsNoTracking` for reads).
   - Extend `DependencyInjection.cs`: bind `JwtOptions` from config section `Jwt`, register repository/hasher/generator/service, and configure `AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(...)` validating issuer, audience, lifetime, signing key; plus `AddAuthorization()`. Throw a clear startup error if `Jwt:Key` is missing.

7. **Api wiring**:
   - `TB.Api/Controllers/AuthController.cs` — thin, `[ApiController] [Route("api/auth")]`:
     - `POST register` → 201 `AuthResponse`.
     - `POST login` → 200 `AuthResponse`.
     - `GET me` → `[Authorize]` → 200 `UserDto`, id from `User.FindFirstValue(ClaimTypes.NameIdentifier)`.
   - `TB.Api/Extensions/ClaimsPrincipalExtensions.cs` → `GetUserId()`.
   - `TB.Api/Exceptions/ApiExceptionHandler.cs` (`IExceptionHandler`) mapping Application exceptions to `ProblemDetails`; register `AddProblemDetails()` + `AddExceptionHandler<>()` and `app.UseExceptionHandler()`.
   - CORS policy allowing `http://localhost:4200` (methods/headers, no credentials).
   - `app.UseAuthentication(); app.UseAuthorization(); app.MapControllers();` (keep `/health`).

8. **Env / config**:
   - `example.env` (tracked) placeholders: `ASPNETCORE_URLS=http://localhost:5000`, `Jwt__Key=[REPLACE-WITH-32+CHAR-RANDOM]`, `Jwt__Issuer=SkillBridge`, `Jwt__Audience=SkillBridge.Client`, `Jwt__ExpiryHours=8`.
   - `.env` (gitignored): generate a real 32+ char `Jwt__Key` plus the same real values. Never commit `.env`.

9. **Seed passwords** — replace `!seed-only-account!` in `SeedData.cs` with a pinned BCrypt hash of the documented demo password `Password123!` (generate one literal once and paste it so the migration is deterministic). Update both demo rows (`employer@skillbridge.demo`, `candidate@skillbridge.demo`).

10. **Migration** — changing `HasData` values requires a migration:
    `dotnet ef migrations add AuthSeedPasswords --project backend/TB.Infrastructure --startup-project backend/TB.Api`.

## Acceptance (backend)

1. `dotnet build backend/SkillBridge.sln` succeeds.
2. `dotnet ef database update --project backend/TB.Infrastructure --startup-project backend/TB.Api`.
3. `dotnet run --project backend/TB.Api`; then:
   - Register candidate → 201 with token/user.
   - Register employer without `companyName` → 400.
   - Register with an existing email → 409.
   - Login (seeded employer/candidate, `Password123!`) → 200; wrong password → 401.
   - `GET /api/auth/me` with token → 200; without → 401.
   - `GET /api/auth/me` with a garbage token → 401.
4. The employer-only 403 path cannot be demonstrated yet (no employer-only endpoint); note this in the PR body. **Left for the team** via PR #3.

## TEAM TASKS (hand-off, PR #1a)

These are intentionally **not** implemented in PR #1a so teammates can pick them up:

- **Automated tests** — create `backend/TB.Tests` (xUnit) and add integration tests for register/login/me (happy path + 400/401/409). Add to the solution. Verify with `dotnet test`.
- **403 role check** — implement the first employer-only endpoint in PR #3 (`feat/employer-jobs`) and add a test asserting a candidate token gets 403.
- **README** — teammates update the README auth section + demo credentials once both PRs land (docs deliberately deferred).

## Git / PR steps (PR #1a)

1. `git switch main; git pull`
2. `git switch -c feat/auth-backend`
3. Implement; run acceptance above.
4. Stage only intended files (do **not** add `*.pdf` or `.env`): `backend/**`, `example.env`.
5. Commit: `feat(auth): add JWT authentication and role-based authorization`.
6. `git push -u origin feat/auth-backend`
7. `gh pr create --base main --head feat/auth-backend --title "feat(auth): JWT authentication and role-based authorization (backend)" --body "<summary, endpoints, migration command, test steps, TEAM TASKS section>"`.

---

# PR #1b — Frontend Auth (`feat/auth-frontend`)

Angular login/register screens, auth service, interceptor, route guards, session persistence, and a minimal authenticated shell. **Base this branch on `main` after PR #1a is merged** so the API base URL and response shapes are stable.

Files under `frontend/src/app/`:

1. `core/auth/auth.models.ts` — `UserRole` union, `AuthUser`, `AuthResponse`, `LoginRequest`, `RegisterRequest`.
2. `core/auth/auth.service.ts` — `inject(HttpClient)`; signals `currentUser`, `token`; methods `login`, `register`, `logout`; computed `isAuthenticated`, `role`; hydrate/persist to `localStorage` (`skillbridge.token`, `skillbridge.user`); expose the API base URL constant (`http://localhost:5000`).
3. `core/auth/auth.interceptor.ts` — functional `HttpInterceptorFn` attaching `Authorization: Bearer <token>` when present; on 401, clear session and redirect to `/login`.
4. `core/auth/auth.guards.ts` — `authGuard` (any signed-in user), `candidateGuard`, `employerGuard` (`CanActivateFn`, redirect unauthenticated to `/login`, wrong role to their home).
5. `features/auth/login/login.ts|.html|.css` and `features/auth/register/register.ts|.html|.css` — standalone, `ReactiveFormsModule`, role toggle (candidate/employer) shown only on register, employer company-name field, loading/error states, wired to `AuthService`.
6. `app.routes.ts` — `''` → `/login`; lazy `loadComponent` for `login`, `register`; **guarded placeholder stubs** for `/jobs` (candidate) and `/create-job`, `/applicants`, `/my-applications` so guards are exercised now. Placeholder routes are minimal stubs only, not real screens.
7. `app.config.ts` — add `provideHttpClient(withInterceptors([authInterceptor]))`; keep `provideRouter(routes)`.
8. `app.html`/`app.ts` — replace the placeholder with a minimal shell: `<router-outlet/>` plus a topbar showing the current user and a Sign out button when authenticated.
9. `app.spec.ts` — the existing default spec will break when `App` is rewritten. Delete or update it so `npm run build` + unit-test discovery stay green. **No new specs are authored** (tests left for the team).

No new frontend npm packages are needed (`@angular/common/http` ships with Angular).

## Acceptance (frontend)

1. `npm install` then `npm run build` in `frontend/` succeeds.
2. `npm start`; register a candidate, sign out, log in, refresh (session persists), and confirm guarded routes redirect correctly; verify loading/error states on bad credentials.
3. Requires the backend running on `http://localhost:5000` (from PR #1a) with CORS allowing `http://localhost:4200`.

## TEAM TASKS (hand-off, PR #1b)

- **Automated tests** — add vitest specs for `AuthService` (login/register/logout/persistence), the interceptor (Bearer header + 401 clear-and-redirect), and the guards (role routing). Deliberately deferred.
- **Real feature screens** — replace the guarded placeholder stubs for `/jobs`, `/create-job`, `/applicants`, `/my-applications` (roadmap PRs #2–#7).
- **README** — teammates document the sign-in flow, demo credentials, and the `http://localhost:5000` ↔ `4200` port alignment.
- **UX polish** — accessible form labels/errors, role toggle styling, topbar responsive behavior.

## Git / PR steps (PR #1b)

1. After #1a merges: `git switch main; git pull`
2. `git switch -c feat/auth-frontend`
3. Implement; run acceptance above.
4. Stage only intended files: `frontend/src/**`.
5. Commit: `feat(auth): add Angular login/register, interceptor, guards, and auth shell`.
6. `git push -u origin feat/auth-frontend`
7. `gh pr create --base main --head feat/auth-frontend --title "feat(auth): Angular authentication UI (frontend)" --body "<summary, screens, TEAM TASKS section>"`.

---

## Risks / notes

- **PR ordering:** #1b depends on #1a's contract (response shapes, port, CORS). Merge #1a first.
- **BCrypt hash in seed:** pin one precomputed hash; the `HasData` change needs the `AuthSeedPasswords` migration or demo logins stay broken.
- **JWT claim mapping:** using `ClaimTypes.*` avoids the `sub` → role/name mapping pitfalls; keep `MapInboundClaims` at defaults.
- **Secret handling:** generated `Jwt:Key` lives only in gitignored `.env`; `example.env` keeps a placeholder. App fails fast at startup if `Jwt:Key` is missing.
- **CORS + port alignment:** backend must allow `http://localhost:4200`; frontend API base is `http://localhost:5000`. No `AllowCredentials` needed (Bearer via localStorage).
- **Empty `Phase1DomainModel` migration** is pre-existing and unrelated; leave it.
- **PDF guides unread** — reconcile any guide-specific conventions during implementation.
- **Don't commit `*.pdf`** or `.env`.

## Backlog — subsequent feature PRs (one each, team-owned)

2. **`feat/candidate-profiles`** — `GET/PUT /api/candidates/me` (profile + skill tag ids), `GET /api/skills`; candidate-only.
3. **`feat/employer-jobs`** — `POST /api/jobs`, `GET /api/employer/jobs`; employer-only. (Also demonstrates the 403 path.)
4. **`feat/job-browsing`** — `GET /api/jobs` with server-side filter by required `skillId`.
5. **`feat/applications`** — `POST /api/jobs/{id}/applications` (duplicate → 400), `GET /api/candidates/me/applications`.
6. **`feat/applicant-review`** — `GET /api/employer/jobs/{id}/applicants` with match % (API-computed); `PATCH /api/applications/{id}/status` enforcing `Received → Shortlisted|Rejected`.
7. **`feat/frontend-flows`** — finish all six wireframe screens, states, and end-to-end journey checks.

Cross-cutting rules for every PR: controllers call services and return DTOs; server-side validation is the source of truth; correct HTTP codes (200/201/400/401/403/404/409); database-side filtering.
