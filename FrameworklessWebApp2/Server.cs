using System;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Web;
using Serilog;


namespace FrameworklessWebApp2
{
    public class Server
    {
        
        private readonly HttpListener _server = new HttpListener();

        public void StartServer()
        {
            var dataManager = new DataManager();
            var httpEngine = new HttpEngine(dataManager);

            var port = GetPortConfig();
            _server.Prefixes.Add($"http://localhost:{port.PortNumber}/"); //URI prefixes 
            _server.Start();
            Console.WriteLine("Start listening");
            
            while (true)
            {
                var context = _server.GetContext();  
                Log.Information($"{context.Request.HttpMethod} {context.Request.Url}");
                //Log.CloseAndFlush();                                     //TODO: is this required here? 
                httpEngine.Process(context);

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