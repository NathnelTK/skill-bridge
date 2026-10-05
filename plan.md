# SkillBridge MVP Roadmap (Condensed)

**Goal:**  
Deliver a working candidate–job matching platform within the 10-hour hackathon window. Judges score the primary journey:  
**Job posted → Browse → Apply once → Employer sees match % and sets status.**

**Architecture:**  
- **Frontend:** Angular 16+, standalone components, reactive forms, JWT guards.  
- **Backend:** ASP.NET Core Web API (.NET 8/9), slim controllers + service classes.  
- **Data:** EF Core code-first, SQL Server/PostgreSQL.  
- **Auth:** JWT with `Candidate` and `Employer` roles.  

**Data Model:**  
- User, Skill, CandidateSkill, Job, JobSkill, Application (with status: Received, Shortlisted, Rejected).  

**Delivery Rules:**  
- Fresh repo, no reused code.  
- Controllers call services, return DTOs.  
- Proper HTTP codes (201, 400, 404, 403, 200).  
- Server-side validation is source of truth.  
- Database filtering, not frontend-only.  
- Every page has loading/empty/error states.  
- Only complete features are visible; unfinished ones listed in README.  

**Core Journey:**  
1. Candidate registers, adds skills.  
2. Employer posts job with required skills.  
3. Candidate browses jobs, applies once.  
4. Employer sees applicants with match %, updates status.  

**Optional Extras (after MVP):**  
- Sort applicants by match %.  
- Filter to “meets all required skills.”  
- Show matched skills explanation.  
- Show match % on candidate job list.  
- Withdraw application (if still Received).  
- Dashboard cards, landing page animation.  

**Out of Scope:**  
File uploads, notifications, saved jobs, recruiter talent search, multi-language, AI scoring, external job distribution, live push.  

**Team Roles:**  
- Lead/Pitch: scope control, demo story.  
- Frontend: Angular components, forms, styling.  
- Backend: controllers, services, DTOs.  
- Database/Full-stack: entities, migrations, seed data.  
- QA/Integrator: edge-case testing, README, demo steps.  

---


