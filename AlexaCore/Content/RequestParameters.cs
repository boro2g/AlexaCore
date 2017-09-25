namespace AlexaCore.Content
{
    public class RequestParameters
    {
        public RequestParameters()
        {
            Parameters = new RequestParameter[0];
        }

        public RequestParameter[] Parameters { get; set; }
    }
}