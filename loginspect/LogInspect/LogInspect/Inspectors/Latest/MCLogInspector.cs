namespace LogInspect.Inspectors.Latest;

public class McLogInspector : Inspector
{
    private const int LogLength = 3;
    
    public override string[] InspectWithPages(string[] log)
    {
        var stacktraces = new List<string>();
        
        foreach (var line in log)
        {
            if (line.Contains("/ERROR") || line.Contains("/FATAL") || line.Contains("/WARN") || line.StartsWith("Exception in thread"))
            {
                var index = Array.IndexOf(log, line);

                if (index + 5 < log.Length)
                {
                    var isStackTrace = true;
                    
                    for (var i = 2; i <= 6; i++)
                    {
                        if (log[index + i].StartsWith($"\t")) continue;
                        isStackTrace = false;
                        
                        break;
                    }
                    
                    if (isStackTrace)
                    {
                        stacktraces.Add(line + $"\n{log[index + 1]}");
                    }
                }
            }

            if (!line.StartsWith($"\t") && !line.StartsWith("Caused by")) continue;
            if (stacktraces.Count <= 0) continue;
                
            stacktraces[^1] += $"\n{line}";
        }
        
        foreach (var stacktrace in stacktraces.ToList())
        {
            var cleanedLines = new List<string>();
            var count = 0;
            
            foreach (var line in stacktrace.Split("\n"))
            {
                if (line.StartsWith("Caused by"))
                {
                    count = 0;
                }

                if (count > LogLength)
                {
                    if (cleanedLines[^1] != "...")
                    {
                        cleanedLines.Add("...");
                    }
                    
                    continue;
                }
                
                cleanedLines.Add(line);
                count++;
            }
            
            stacktraces[stacktraces.IndexOf(stacktrace)] = string.Join("\n", cleanedLines);
        }
        
        return stacktraces.ToArray();
    }
}