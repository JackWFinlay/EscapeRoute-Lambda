using System.Collections.Generic;
using Newtonsoft.Json;

namespace EscapeRoute_Lambda
{
    public class Request
    {
        [JsonProperty("queryStringParameters")]
        public Dictionary<string, string> QueryStringParameters { get; set; }
    }
}