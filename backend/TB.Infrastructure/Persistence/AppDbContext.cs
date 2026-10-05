using Microsoft.EntityFrameworkCore;
using TB.Domain.Entities;

namespace TB.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobSkill> JobSkills => Set<JobSkill>();

    public DbSet<TB.Domain.Entities.Application> Applications =>
    Set<TB.Domain.Entities.Application>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(user => user.Id);

            entity.Property(user => user.Email)
                .HasMaxLength(320)
                .IsRequired();

            entity.HasIndex(user => user.Email)
                .IsUnique();

            entity.Property(user => user.PasswordHash)
                .HasMaxLength(512)
                .IsRequired();

            entity.Property(user => user.Role)
                .HasMaxLength(32)
                .IsRequired();

            entity.Property(user => user.FullName)
                .HasMaxLength(160)
                .IsRequired();

            entity.Property(user => user.CompanyName)
                .HasMaxLength(160);

            entity.Property(user => user.Location)
                .HasMaxLength(160);

            entity.HasData(SeedData.Users);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("skills");

            entity.HasKey(skill => skill.Id);

            entity.Property(skill => skill.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(skill => skill.Name)
                .IsUnique();

            entity.HasData(SeedData.Skills);
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.ToTable("candidate_skills");

            entity.HasKey(candidateSkill =>
                new
                {
                    candidateSkill.CandidateId,
                    candidateSkill.SkillId
                });

            entity.HasOne(candidateSkill => candidateSkill.Candidate)
                .WithMany()
                .HasForeignKey(candidateSkill => candidateSkill.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(candidateSkill => candidateSkill.Skill)
                .WithMany()
                .HasForeignKey(candidateSkill => candidateSkill.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(SeedData.CandidateSkills);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable("jobs");

            entity.HasKey(job => job.Id);

            entity.Property(job => job.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(job => job.Description)
                .IsRequired();

            entity.Property(job => job.Location)
                .HasMaxLength(160);

            entity.Property(job => job.JobType)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(job => job.IsActive)
                .IsRequired();

            entity.HasOne(job => job.Employer)
                .WithMany()
                .HasForeignKey(job => job.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(SeedData.Jobs);
        });

        modelBuilder.Entity<JobSkill>(entity =>
        {
            entity.ToTable("job_skills");

            entity.HasKey(jobSkill =>
                new
                {
                    jobSkill.JobId,
                    jobSkill.SkillId
                });

            // IMPORTANT:
            // This connects JobSkill.Job to Job.JobSkills.
            // It prevents EF Core from creating the shadow JobId1 column.
            entity.HasOne(jobSkill => jobSkill.Job)
                .WithMany(job => job.JobSkills)
                .HasForeignKey(jobSkill => jobSkill.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(jobSkill => jobSkill.Skill)
                .WithMany()
                .HasForeignKey(jobSkill => jobSkill.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(SeedData.JobSkills);
        });

        modelBuilder.Entity<TB.Domain.Entities.Application>(entity =>
        {
            entity.ToTable("applications");

            entity.HasKey(application => application.Id);

            entity.Property(application => application.Status)
                .HasMaxLength(32)
                .IsRequired();

            entity.HasIndex(application =>
                new
                {
                    application.JobId,
                    application.CandidateId
                })
                .IsUnique();

            entity.HasOne(application => application.Job)
                .WithMany()
                .HasForeignKey(application => application.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(application => application.Candidate)
                .WithMany()
                .HasForeignKey(application => application.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}