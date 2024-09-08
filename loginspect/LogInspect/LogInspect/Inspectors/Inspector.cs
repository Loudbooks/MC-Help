namespace LogInspect.Inspectors;

public abstract class Inspector
{
    public virtual string Inspect(string[] log)
    {
        throw new NotImplementedException();
    }

    public virtual string[] InspectWithPages(string[] log)
    {
        throw new NotImplementedException();
    }
}