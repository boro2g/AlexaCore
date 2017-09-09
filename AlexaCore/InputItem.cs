namespace AlexaCore
{
    public class InputItem
    {
        public string Value { get; }
        public string[] Tags { get; }

        public InputItem(string value, string[] tags = null)
        {
            Value = value;
            Tags = tags ?? new string[0];
        }
    }
}
