using System;
using System.Net;
using FrameworklessWebApp2.Controllers;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Web.HttpResponse;

namespace FrameworklessWebApp2.Web
{
    public static class RequestExceptionExtensions
    {
        public static ResponseMessage AsHttpResponse(this RequestException e, Uri uri)
        {
            return e switch
            {
                InvalidOperationsException _ => new ResponseMessage(HttpStatusCode.BadRequest, e.Message + uri), //TODO: what does the _ mean? 
                ObjectNotFoundException _ => new ResponseMessage(HttpStatusCode.NotFound, "Page not found: " + uri),
                HttpRequestException exception => new ResponseMessage(exception.StatusCode, exception.Message + uri),
                _ => throw new Exception("Not a RequestException")
            };
        }
    }
}