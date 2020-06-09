using System;

namespace FrameworklessWebApp2.DataAccess
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message) : base(message)
        {
            
        }
    }
}