using System;
using System.Net;
using FrameworklessWebApp2.Web;

namespace FrameworklessWebApp2
{
    public interface IHttpListenerContextWrapper
    {
        Uri RequestUri { get;  }
    }

    class HttpListenerContextWrapper : IHttpListenerContextWrapper
    {
        private HttpListenerContext _context;

        public Uri RequestUri => _context.Request.Url;
    }

    // public class A
    // {
    //     void B()
    //     {
    //         var httpEngine = new HttpEngine();
    //         
    //         httpEngine.Process(new HttpListenerContextWrapper(theOriginalContext));
    //         
    //         // in the test
    //         httpEngine.Process(new StubHttpListontext()); // constructor with mock info 
    //     }
    // }
}