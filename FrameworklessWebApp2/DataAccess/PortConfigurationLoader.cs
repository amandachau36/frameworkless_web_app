using System;
using Microsoft.Extensions.Configuration;

namespace FrameworklessWebApp2
{
    public static class PortConfigurationLoader
    {
        public static PortConfig LoadPortConfig(string path)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            
            var portNumber = ProcessPortNumber(config["Port"]);
            
            return new PortConfig(portNumber);
        }
        
        private static int ProcessPortNumber(string portNumber)
        {
            if (!int.TryParse(portNumber, out var result))
            {
                throw new ArgumentException(
                    $"Not a valid {portNumber}. Must be an int.");
            }

            return result;
        }
        
    }
}