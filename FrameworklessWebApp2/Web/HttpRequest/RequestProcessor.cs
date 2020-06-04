using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using FrameworklessWebApp2.Controllers;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Web.HttpRequest
{
    public class RequestProcessor
    {
        public static HttpVerb GetVerb(string httpMethod)
        {
            if (Enum.TryParse(httpMethod, true, out HttpVerb verb))
            {
                return verb;
            }
            
            throw new HttpRequestException($"Invalid http method: {httpMethod} for ", HttpStatusCode.MethodNotAllowed);
        }
        
        //https://stackoverflow.com/questions/39386586/c-sharp-generic-interface-and-factory-pattern
        public static object GetController(string controllerString, DataManager dataManager)
        {
            return controllerString switch
            {
                "users" => new UsersController(dataManager), //UsersController<user>(dataManager)
                _ => throw new HttpRequestException("Page not found: ", HttpStatusCode.NotFound)
            };
        }
        
        public static int? GetId(List<string> uriSegments)
        {
            
            if (uriSegments.Count > 2 && int.TryParse(uriSegments[2], out var id))
                return id;

            return null; //TODO: case users/cats Throw exception 
        }
        
        public static object GetModel(string route, HttpListenerRequest request)  //TODO: how to return this IModel
        {
            var json = ReadBody(request);

            if(route == "users") 
                return JsonConvert.DeserializeObject<User>(json);   //TODO: need to throw exception and try/catch exception here if it can't be DeSerialized 
            
            throw new HttpRequestException($"Model not found: {route}. ", HttpStatusCode.BadRequest ); //TODO: 404 
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