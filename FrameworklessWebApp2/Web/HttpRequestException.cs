using System;
using System.Net;
using FrameworklessWebApp2.Web;

namespace FrameworklessWebApp2
{
    public class HttpRequestException : RequestException
    {
        public HttpStatusCode StatusCode { get; }

        public HttpRequestException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

