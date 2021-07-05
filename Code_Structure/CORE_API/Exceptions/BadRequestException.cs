using System;

namespace NEWS_API.Cores.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string mess) : base(mess) { }
    }
}
