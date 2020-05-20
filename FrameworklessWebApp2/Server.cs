using System;
using System.IO;
using System.Net;

namespace FrameworklessWebApp2
{
    public static class Server
    {
        private static readonly HttpListener _server = new HttpListener();

        public static void StartServer()
        {
            var port = GetPortConfig();
            
            _server.Prefixes.Add($"http://localhost:{port.PortNumber}/"); //URI prefixes 
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

        private static PortConfig GetPortConfig()
        {
            return PortConfigurationLoader.LoadPortConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "PortConfiguration.json"));  //TODO: is this correct so that program.cs doesn't know about the data access Layer? 
        }
    }
}