using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public static class Request
    {
        public static List<User> Users { get; } = new List<User>();
        public static void Process(HttpListenerContext context)
        {
            var request = context.Request; 
            var response = context.Response;
           
            switch (request.Url.AbsolutePath)
            {
                case "/":
                    response.StatusCode = (int) HttpStatusCode.OK;  
                    Response.Send("Welcome to the Home Page", context);
                    break;
                case "/users":
                    switch (request.HttpMethod)
                    {
                        case "GET":
                            Console.WriteLine("Get users");
                            response.StatusCode = (int) HttpStatusCode.OK;  
                            var htmlMessage = Html.Wrap("All Users", "<h1></h1>"); //But this should probably be done in the front end?
                            Response.Send(htmlMessage, context);
                            break;
                        case "POST": 
                            Console.WriteLine("posting to /Users");
                            var body = request.InputStream;
                            
                            var reader = new StreamReader(body, request.ContentEncoding);

                            var json = reader.ReadToEnd();
                            
                            var user = JsonConvert.DeserializeObject<User>(json);
                            
                            Users.Add(user);

                            response.StatusCode = (int) HttpStatusCode.Created;
                            
                            DataManager.WriteToTextFile();
                            Console.WriteLine("============\n" + user.Name + $"({user.Username})");  //TODO: make logging better
                            Response.Send(user.Name, context);  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay
                            break;
                    }
                    break;
                case "/countries":
                    response.StatusCode = (int) HttpStatusCode.OK;  
                    Response.Send("All Countries", context);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;  
                    Response.Send("Page not found: " + request.Url, context);  
                    break;
                
            }
        }
    }
}