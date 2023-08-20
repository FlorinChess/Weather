namespace Weather.Core.Exceptions
{
    public sealed class ServerSideIssueException : WeatherClientException
    {
        public override string Message => "Server-side error! Please try again later!";
    }
}
