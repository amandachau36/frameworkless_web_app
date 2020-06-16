using System;

namespace FrameworklessWebApp2.Web
{
    public class RequestException : Exception
    {
        protected RequestException(string message) : base(message)
        {
            
        }
    }
}