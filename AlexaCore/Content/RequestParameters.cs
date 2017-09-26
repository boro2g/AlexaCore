using System;
using System.Linq;
using System.Net;

namespace AlexaCore.Content
{
    public class RequestParameters
    {
        public RequestParameters()
        {
            Parameters = new RequestParameter[0];
        }

        public RequestParameters(RequestParameter[] parameters)
        {
            Parameters = parameters;
        }

        public RequestParameter[] Parameters { get; set; }

        public string ToQueryString(string prefix = "?")
        {
            return Parameters.Length > 0
                ? $"{prefix}{String.Join("&", Parameters.Select(a => $"{WebUtility.UrlEncode(a.Key)}={WebUtility.UrlEncode(a.Value)}"))}"
                : "";
        }
    }
}