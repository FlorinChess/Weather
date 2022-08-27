namespace Weather.Core.Exceptions
{
    public class ApiError
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public ushort code { get; set; }
        public string message { get; set; }
    }
}
