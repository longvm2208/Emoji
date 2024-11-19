using Firebase.Analytics;

public class CachedFirebaseEvent
{
    public string name;
    public Parameter[] parameters;

    public CachedFirebaseEvent(string name, Parameter[] parameters)
    {
        this.name = name;
        this.parameters = parameters;
    }
}
