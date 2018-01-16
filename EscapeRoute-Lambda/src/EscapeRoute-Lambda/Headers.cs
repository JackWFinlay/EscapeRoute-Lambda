using Newtonsoft.Json;

namespace EscapeRoute_Lambda
{
    public class Headers
    {
        [JsonProperty(PropertyName = "Content-Type")]
        public string ContentType;

        //[JsonProperty(PropertyName = "Access-Control-Allow-Origin")]
        //public string access = "*";

        public Headers(string format)
        {
            if (format.Equals("json"))
            {
                ContentType = "application/json";
            }
        }
    }
}