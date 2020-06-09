using System;
using System.IO;
using System.Text;

namespace FrameworklessWebApp2
{
    public interface IHttpListenerRequestWrapper
    {
        Uri Uri { get;  }
        
        string HttpMethod { get; }
        
        Stream InputStream { get; }
        
        Encoding ContentEncoding { get; }
    }
}