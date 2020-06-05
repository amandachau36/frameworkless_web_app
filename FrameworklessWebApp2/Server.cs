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
        private readonly ILogger _logger;
        private readonly HttpListener _server;
        public Server(ILogger logger)
        {
            _logger = logger;
            _server = new HttpListener();
        }
     

        public void StartServer()
        {
            var dataManager = new DataManager(_logger);
            var httpEngine = new HttpEngine(dataManager, _logger);

            var port = GetPortConfig();
            
            _server.Prefixes.Add($"http://localhost:{port.PortNumber}/"); //URI prefixes 
            _server.Start();
            _logger.Information("Start listening on " + port.PortNumber);
            
            while (true) //TODO: Instead of true. Stop server when requested. 
            {
                var context = _server.GetContext();  
                _logger.Debug($"{context.Request.HttpMethod} {context.Request.Url}");
                var responseMessage = httpEngine.Process(context);
                httpEngine.Send(responseMessage, context.Response);
                

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