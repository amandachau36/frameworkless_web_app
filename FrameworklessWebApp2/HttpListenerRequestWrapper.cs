using System;
using System.IO;
using System.Net;
using System.Text;
using FrameworklessWebApp2.Web;

namespace FrameworklessWebApp2
{
    class HttpListenerRequestWrapper : IHttpListenerRequestWrapper
    {
        private readonly HttpListenerRequest _request;
        public Uri Uri => _request.Url;
        public string HttpMethod => _request.HttpMethod;
        public Stream InputStream => _request.InputStream;
        public Encoding ContentEncoding => _request.ContentEncoding;


        public HttpListenerRequestWrapper(HttpListenerRequest request)
        {
            _request = request;
        }
    }
    
}