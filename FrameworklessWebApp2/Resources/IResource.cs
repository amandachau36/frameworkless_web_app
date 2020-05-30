using System.Collections.Generic;
using System.Net;

namespace FrameworklessWebApp2.Resources
{
    public interface IResource
    { 
        string Get();
        string Get(int? id);
        void Put();
        string Post(HttpListenerContext context);
        void Delete();
    }
}