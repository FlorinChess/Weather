using System;

namespace Weather.Core.Exceptions
{
    public class LocationNotFoundException : Exception
    {
        public string Message { get; set; }
        public LocationNotFoundException(string message)
        {
            Message = message;
        }
    }
}
