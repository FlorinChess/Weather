using System;

namespace Weather.Core.Exceptions
{
    public abstract class WeatherClientException : Exception
    {
        public abstract override string Message { get; }
    }
}
