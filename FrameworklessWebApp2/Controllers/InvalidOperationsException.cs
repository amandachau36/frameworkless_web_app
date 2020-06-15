using FrameworklessWebApp2.Web;

namespace FrameworklessWebApp2.Controllers
{
    public class InvalidOperationsException : RequestException
    {
        public InvalidOperationsException(string message) : base(message)
        {
            
        }
    }
}