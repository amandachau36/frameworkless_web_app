using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using FrameworklessWebApp2.Controllers;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Models;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public class RequestProcessor
    {
        public static HttpVerb GetVerb(string httpMethod)
        {
            if (Enum.TryParse(httpMethod, true, out HttpVerb verb))
            {
                return verb;
            }
            
            throw new HttpRequestException("Invalid http method: " + httpMethod);
        }
        //https://stackoverflow.com/questions/39386586/c-sharp-generic-interface-and-factory-pattern
        // public static object GetController<T>(string route, DataManager dataManager) where T : class
        // {
        //
        //     if (route == "users")
        //         return (IController<T>) new UsersController<User>(dataManager);
        //     
        //     throw new HttpRequestException($"Controller not found: " + route); //TODO: notFOUndException
        // } 
        
        
        public static object GetController(string controllerString, DataManager dataManager)
        {
            return controllerString switch
            {
                "users" => new UsersController<User>(dataManager),
                _ => throw new InvalidOperationException("Invalid type specified.")
            };
        }
        
        public static int? GetId(List<string> uriSegments)
        {
            
            if (uriSegments.Count > 2 && int.TryParse(uriSegments[2], out var id))
                return id;

            return null; //TODO: case users/cats Throw exception 
        }
        
        public static User GetModel(string route, HttpListenerRequest request)
        {
            var json = ReadBody(request);
            
            if(route == "users") 
                return JsonConvert.DeserializeObject<User>(json); 
            
            throw new HttpRequestException($"Model not found: " + route); //TODO: 404 
        }

        public static List<string> GetProcessedUriSegments(Uri uri)
        {
            var segments = uri.Segments;

            return segments.Select(s => s.TrimEnd('/')).ToList();
        }
        
        private static string ReadBody(HttpListenerRequest request)
        {
            var body = request.InputStream;  //Controller
                            
            var reader = new StreamReader(body, request.ContentEncoding);

            return reader.ReadToEnd();
        }
        
        
        
        


    }
}