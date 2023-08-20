namespace Weather.Core.Exceptions
{
    public sealed class InvalidWeatherLocationException : WeatherClientException
    {
        public override string Message => "Invalid weather location! Please enter a different location!";
    }
}
