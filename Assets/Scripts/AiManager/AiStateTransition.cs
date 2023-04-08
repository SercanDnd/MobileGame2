public class AiStateTransition
{
    public IAiManager From { get; }
    public IAiManager To { get; }
    public System.Func<bool> Condition { get; }

    public AiStateTransition(IAiManager from,IAiManager to,System.Func<bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }
    

}
