namespace AlexaCore.Content
{
    public class ContentResponse
    {
        public IIntentContent RequestResponse { get; }
        public string Key { get; }

        public ContentResponse(IIntentContent requestResponse, string key)
        {
            RequestResponse = requestResponse;
            Key = key;
        }

        public string FormattedContent { get; set; }
        public bool DefaultText { get; set; }

        public override string ToString()
        {
            return FormattedContent;
        }
    }
}