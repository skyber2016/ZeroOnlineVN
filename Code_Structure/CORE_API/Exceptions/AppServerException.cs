using System;

namespace CORE_API.Exceptions
{
    public class AppServerException : Exception
    {
        public AppServerException(string mess): base(mess)
        {

        }
    }
}
