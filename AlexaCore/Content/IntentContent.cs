namespace AlexaCore.Content
{
    public class IntentContent : IIntentContent
    {
        public IntentContent()
        {
            Default = false;
        }

        public string IntentName { get; set; }
        public string Content { get; set; }
        public string Variation { get; set; }
        public int FormatSlots { get; set; }
        public string Key { get; set; }
        public bool Default { get; set; }
    }
}