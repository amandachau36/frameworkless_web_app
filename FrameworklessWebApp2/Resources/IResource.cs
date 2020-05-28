using System.Collections.Generic;
using System.Net;

namespace FrameworklessWebApp2.Resources
{
    public interface IResource
    { 
        List<User> Get();
        void Put();
        List<User> Post(HttpListenerContext context);
        void Delete();
    }
}