using System.Collections.Generic;
using System.Net;

namespace FrameworklessWebApp2.Resources
{
    public interface IResource
    { 
        string Get();
        string Get(int? id);
        string Put(int? id, HttpListenerContext context);
        string Post(HttpListenerContext context);
        void Delete();
    }
}