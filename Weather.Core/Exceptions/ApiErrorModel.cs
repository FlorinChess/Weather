using Newtonsoft.Json;

namespace Weather.Core.Exceptions
{
    public class ApiError
    {
        [JsonProperty(PropertyName = "error")]
        public Error error { get; set; }
    }

    public class Error
    {
        [JsonProperty(PropertyName = "code")]
        public ushort code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string message { get; set; }
    }
}
