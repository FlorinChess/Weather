using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
