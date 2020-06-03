using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;

namespace FrameworklessWebApp2
{
    public static class Headers
    {
        public static void AddAllHeaders(HttpListenerResponse response)
        {
            var headers = new Dictionary<string, string>  // can make this into a factory if I require different combos
            {
                {"Content-Type", "application/json"}, 
            };
            
            foreach (var (key, value) in headers)
            {
                response.Headers.Add(key, value);    
            }
       
        }

    }
}