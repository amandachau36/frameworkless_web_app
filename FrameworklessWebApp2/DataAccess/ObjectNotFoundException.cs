using System;
using FrameworklessWebApp2.Web;

namespace FrameworklessWebApp2.DataAccess
{
    public class ObjectNotFoundException : RequestException
    {
        public ObjectNotFoundException(string message) : base(message)
        {
            
        }
    }
}