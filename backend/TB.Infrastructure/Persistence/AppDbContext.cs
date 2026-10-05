using Microsoft.EntityFrameworkCore;
using TB.Domain.Entities;
using TB.Domain.Enums;

namespace TB.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobSkill> JobSkills => Set<JobSkill>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Email).HasMaxLength(320).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(user => user.Role)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
            entity.Property(user => user.FullName).HasMaxLength(160).IsRequired();
            entity.Property(user => user.CompanyName).HasMaxLength(160);
            entity.Property(user => user.Location).HasMaxLength(160);
            entity.HasMany(user => user.CandidateSkills)
                .WithOne(candidateSkill => candidateSkill.Candidate)
                .HasForeignKey(candidateSkill => candidateSkill.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(user => user.PostedJobs)
                .WithOne(job => job.Employer)
                .HasForeignKey(job => job.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(user => user.Applications)
                .WithOne(application => application.Candidate)
                .HasForeignKey(application => application.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasData(SeedData.Users);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("skills");
            entity.HasKey(skill => skill.Id);
            entity.Property(skill => skill.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(skill => skill.Name).IsUnique();
            entity.HasData(SeedData.Skills);
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.ToTable("candidate_skills");
            entity.HasKey(candidateSkill => new { candidateSkill.CandidateId, candidateSkill.SkillId });
            entity.HasOne(candidateSkill => candidateSkill.Skill)
                .WithMany(skill => skill.Candidates)
                .HasForeignKey(candidateSkill => candidateSkill.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasData(SeedData.CandidateSkills);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable("jobs");
            entity.HasKey(job => job.Id);
            entity.Property(job => job.Title).HasMaxLength(200).IsRequired();
            entity.Property(job => job.Description).IsRequired();
            entity.Property(job => job.Location).HasMaxLength(160).IsRequired();
            entity.HasData(SeedData.Jobs);
        });

        modelBuilder.Entity<JobSkill>(entity =>
        {
            entity.ToTable("job_skills");
            entity.HasKey(jobSkill => new { jobSkill.JobId, jobSkill.SkillId });
            entity.HasOne(jobSkill => jobSkill.Job)
                .WithMany(job => job.RequiredSkills)
                .HasForeignKey(jobSkill => jobSkill.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(jobSkill => jobSkill.Skill)
                .WithMany(skill => skill.Jobs)
                .HasForeignKey(jobSkill => jobSkill.SkillId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasData(SeedData.JobSkills);
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("applications");
            entity.HasKey(application => application.Id);
            entity.Property(application => application.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
            entity.HasIndex(application => new { application.JobId, application.CandidateId }).IsUnique();
            entity.HasOne(application => application.Job)
                .WithMany(job => job.Applications)
                .HasForeignKey(application => application.JobId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
