using System;

namespace libermedical.CustomExceptions
{
    public class BadResponseException : Exception
    {
        public BadResponseException(string message) : base(message) { }
    }
}
