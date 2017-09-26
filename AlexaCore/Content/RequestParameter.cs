namespace AlexaCore.Content
{
    public class RequestParameter
    {
        public RequestParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}