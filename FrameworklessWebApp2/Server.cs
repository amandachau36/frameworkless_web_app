using System;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;


namespace FrameworklessWebApp2
{
    public class Server
    {
        
        private readonly HttpListener _server = new HttpListener();

        public void StartServer()
        {
            var dataManager = new DataManager();
            var request = new RequestProcessor(dataManager);

            var port = GetPortConfig();
            _server.Prefixes.Add($"http://localhost:{port.PortNumber}/"); //URI prefixes 
            _server.Start();
            Console.WriteLine("Start listening");
            
            while (true)
            {
                var context = _server.GetContext();  
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");

                request.Process(context);

            }
            _server.Stop();  // never reached...
        }

        private PortConfig GetPortConfig()
        {
            return PortConfigurationLoader.LoadPortConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataAccess",
                "PortConfiguration.json")); 
        }
    }
}