using System;
using System.Net;

namespace FrameworklessWebApp2
{
    public static class Server
    {
        private const int Port = 8080;
        
        private static readonly HttpListener _server = new HttpListener();

        public static void StartServer()
        {
            _server.Prefixes.Add($"http://localhost:{Port}/"); //URI prefixes 
            _server.Start();
            Console.WriteLine("Start listening");
            while (true)
            {
                var context = _server.GetContext();  
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");
                Request.Process(context);

            }
            _server.Stop();  // never reached...
        }
    }
}