using System.Text.Json;
using System.Text.RegularExpressions;

namespace LogInspect.Inspectors.HJT;

public class HjtInspector : Inspector
{
    private readonly List<HjtFlag> _flags = [];
    
    public HjtInspector()
    {
        var json = File.ReadAllText($"{Program.DataPath}/HjtFlags.json");
        var flags = JsonSerializer.Deserialize<List<HjtFlag>>(json);
        
        if (flags == null)
        {
            throw new Exception("Failed to deserialize HJT flags");
        }
        
        if (!File.Exists($"{Program.DataPath}/HjtPatches.json"))
        {
            File.WriteAllText($"{Program.DataPath}/HjtPatches.json", "[]");
        }
        
        var patchesJson = File.ReadAllText($"{Program.DataPath}/HjtPatches.json");
        var patches = JsonSerializer.Deserialize<List<HjtFlag>>(patchesJson);
        
        if (patches == null)
        {
            throw new Exception("Failed to deserialize HJT patches");
        }
        
        flags.AddRange(patches);
        _flags.AddRange(flags);
    }
    
    public override string Inspect(string[] log)
    {
        var hits = new List<HjtFlag>();
        
        foreach (var line in log)
        {
            foreach (var flag in _flags.Where(flag => hits.All(hit => hit.Name != flag.Name)).Where(flag => Regex.IsMatch(line, flag.MatchCriteria)))
            {
                hits.Add(flag);
            }
        }
        
        hits.Sort((a, b) => b.Severity.CompareTo(a.Severity));
        
        return Serialize(hits);
    }
    
    private static string Serialize(List<HjtFlag> issues)
    {
        {
            issues.Sort((a, b) => b.Severity.CompareTo(a.Severity));

            var lines = new List<string>();
            if (issues.Count == 0)
            {
                lines.Add("No issues found");
            }
            else
            {
                lines.Add(issues.Count == 1 ? "Found **1** issue:" : $"Found **{issues.Count}** issues:");
                lines.Add("");

                foreach (var issue in issues)
                {
                    switch (issue.Severity)
                    {
                        case 0:
                            lines.Add($":green_circle: **{issue.Name}** ({issue.Category}): {issue.Description}");
                            break;
                        case 1:
                            lines.Add($":orange_circle: **{issue.Name}** ({issue.Category}): {issue.Description}");
                            break;
                        case 2:
                            lines.Add($":red_circle: **{issue.Name}** ({issue.Category}): {issue.Description}");
                            break;
                        case 3:
                            lines.Add($":red_circle: **{issue.Name}** ({issue.Category}): {issue.Description}");
                            break;
                    }
                }
            }

            return string.Join("\n", lines);
        }
    }
}