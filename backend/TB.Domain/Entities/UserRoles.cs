namespace TB.Domain.Entities;

public static class UserRoles
{
    public const string Candidate = "Candidate";
    public const string Employer = "Employer";

    public static bool IsValid(string? role) => role is Candidate or Employer;
}