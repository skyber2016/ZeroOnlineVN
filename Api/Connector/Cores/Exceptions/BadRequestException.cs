using System;

namespace Forum_API.Cores.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string mess) : base(mess) { }
    }
}
