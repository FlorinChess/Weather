using System;

namespace Weather.Core.Exceptions
{
    public sealed class LocationNotFoundException : Exception
    {
        public LocationNotFoundException(string message) : base(message) { }
    }
}
