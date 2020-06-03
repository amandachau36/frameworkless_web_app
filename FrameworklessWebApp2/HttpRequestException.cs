using System;
using System.Net;

namespace FrameworklessWebApp2
{
    public class HttpRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpRequestException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

