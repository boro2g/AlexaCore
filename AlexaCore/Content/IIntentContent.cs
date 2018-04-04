namespace AlexaCore.Content
{
    public interface IIntentContent
    {
        string Content { get; }
        bool Default { get; }
        bool ShouldEndSession { get; }
    }
}