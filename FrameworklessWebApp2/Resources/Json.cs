using System.IO;
using System.Net;
using System.Xml.Linq;

namespace FrameworklessWebApp2.Resources
{
    public class Json
    {
        public static string Read(HttpListenerContext context)
        {
            var body = context.Request.InputStream;  //Controller
                            
            var reader = new StreamReader(body, context.Request.ContentEncoding);

            return reader.ReadToEnd();
        }
    }
}