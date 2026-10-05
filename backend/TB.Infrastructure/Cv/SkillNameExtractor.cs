using System.Text.RegularExpressions;
using TB.Application.Abstractions;
using TB.Domain.Entities;

namespace TB.Infrastructure.Cv;

/// <summary>
/// Matches CV text to the known skill vocabulary. Handles common synonyms and
/// punctuation variants (for example "C sharp" / "csharp" -> "C#") so that CV
/// wording lines up with the canonical skill names stored in the database.
/// </summary>
public sealed partial class SkillNameExtractor : ISkillExtractor
{
    private static readonly IReadOnlyDictionary<string, string[]> Aliases =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["C#"] = ["c#", "c sharp", "csharp", "c-sharp"],
            ["ASP.NET Core"] = ["asp.net core", "asp.net", "aspnet core", "aspnet", "asp net core", ".net core"],
            ["PostgreSQL"] = ["postgresql", "postgres", "psql"],
            ["Angular"] = ["angular", "angularjs", "angular.js"],
            ["TypeScript"] = ["typescript", "type script", "ts"],
            ["REST APIs"] = ["rest api", "rest apis", "restful", "rest", "web api"],
            ["Docker"] = ["docker", "containerization", "containerisation"],
            ["Git"] = ["git", "github", "gitlab", "version control"]
        };

    public IReadOnlyList<Skill> ExtractSkills(string cvText, IReadOnlyList<Skill> knownSkills)
    {
        if (string.IsNullOrWhiteSpace(cvText))
        {
            return [];
        }

        var normalizedText = Normalize(cvText);
        var matches = new List<Skill>();

        foreach (var skill in knownSkills)
        {
            var candidates = Aliases.TryGetValue(skill.Name, out var aliases)
                ? aliases
                : [skill.Name];

            if (candidates.Any(candidate => ContainsWord(normalizedText, candidate)))
            {
                matches.Add(skill);
            }
        }

        return matches;
    }

    private static bool ContainsWord(string normalizedText, string candidate)
    {
        var pattern = $@"(?<![a-z0-9]){Regex.Escape(Normalize(candidate))}(?![a-z0-9])";
        return Regex.IsMatch(normalizedText, pattern);
    }

    private static string Normalize(string value) =>
        value.ToLowerInvariant();
}
