using System.Collections.Generic;

namespace FrameworklessWebApp2.Resources
{
    public interface IResource
    { 
        List<User> Get();
        void Put();
        void Post();
        void Delete();
    }
}