using System;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public static class Request
    { 
        public static void Process(HttpListenerContext context) 
        {
            switch (context.Request.Url.AbsolutePath)
            {
                case "/":
                    Response.Send("Welcome to the Home Page", context);
                    break;
                case "/users":
                    switch (context.Request.HttpMethod)
                    {
                        case "GET":
                            Console.WriteLine("Get users");
                            
                            Response.Send("All Users", context);
                            break;
                        case "POST": 
                            Console.WriteLine("posting to /Users");
                            var body = context.Request.InputStream;
                            
                            var reader = new StreamReader(body, context.Request.ContentEncoding);

                            var json = reader.ReadToEnd();
                            var user = JsonConvert.DeserializeObject<User>(json);
                            
                            //TODO: save to textFile
                           
                            context.Response.StatusCode = 201;  
                            
                            Console.WriteLine("============\n" + user.Name + $"({user.Username})");  //TODO: make logging better
                            Response.Send(user.Name, context);  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay
                            break;
                    }
                    break;
                case "/countries":
                    Response.Send("All Countries", context);
                    break;
                default:
                    Response.Send("This should be a 404 page", context); //TODO: should this be a default ?? list of routes and raise exception  
                    break;
                //context.Response.StatusCode 
            }
        }
    }
}