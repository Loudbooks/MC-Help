using System.Text.Json.Serialization;

namespace LogInspect.Inspectors.HJT;

public class HjtFlag
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("match_criteria")] public string MatchCriteria { get; set; } = null!;

    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    
    [JsonPropertyName("category")] public string Category { get; set; } = null!;
    
    [JsonPropertyName("severity")] public int Severity { get; set; } = 0;
    
    [JsonPropertyName("severity_description")] public string SeverityDescription { get; set; } = null!;
}