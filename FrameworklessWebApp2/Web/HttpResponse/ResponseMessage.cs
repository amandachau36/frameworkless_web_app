using System.Net;

namespace FrameworklessWebApp2.Web.HttpResponse
{
    public class ResponseMessage
    {
        public HttpStatusCode StatusCode { get; }
        public object Message { get; }

        public ResponseMessage(HttpStatusCode statusCode, object message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}