using Newtonsoft.Json;

namespace EscapeRoute_Lambda
{
    public class Response
    {
        [JsonProperty(Order = 1, PropertyName = "IsBase64Encoded")]
        public bool IsBase64Encoded;

        [JsonProperty(Order = 2, PropertyName = "headers")]
        public Headers Headers {get; set;}

        [JsonProperty(Order = 3, PropertyName = "statusCode")]
        public int StatusCode;

        [JsonProperty(Order = 4, PropertyName = "body")]
        public string Body {get; set;}

        public Response(int statusCode, string format, string body){

            Headers = new Headers(format);
            Body = body;
            StatusCode = statusCode;
        }
    }
}