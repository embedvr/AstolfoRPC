namespace AstolfoRPC;

public class PresenceStateList : List<PresenceState> { }

public class PresenceState
{
    public string Text { get; set; }
    public int Delay { get; set; }
}