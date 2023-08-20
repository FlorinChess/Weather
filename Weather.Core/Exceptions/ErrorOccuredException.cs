using System;

namespace Weather.Core.Exceptions
{
    public sealed class ErrorOccuredException : WeatherClientException
    {
        public override string Message => "An error occured! Please try again!";
    }
}
