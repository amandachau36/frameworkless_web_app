using System.Collections.Generic;
using System.Net;

namespace FrameworklessWebApp2.Resources
{
    public interface IResource
    { 
        string Get();
        
        string Put(HttpListenerContext context);
        string Post(HttpListenerContext context);
        void Delete();
    }
}