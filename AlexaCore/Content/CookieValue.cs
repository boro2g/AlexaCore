namespace AlexaCore.Content
{
    public class CookieValue
    {
        public string Value { get; set; }

        public string Key { get; set; }

        public override string ToString()
        {
            return $"{Key}={Value}";
        }
    }
}