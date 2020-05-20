using System;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;

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

                            var responseBody = reader.ReadToEnd();
                            //JsonConvert<Model>.Deserialize
                            context.Response.StatusCode = 201;
                            Console.WriteLine("============\n" + responseBody);
                            Response.Send(responseBody, context);  // TODO: why does this have to happen? 
                            break;
                    }
                    break;
                case "/countries":
                    Response.Send("All Countries", context);
                    break;
                default:
                    Response.Send("This should be a 404 page", context); //TODO: should this be a default ?? list of routes and raise excpetion  
                    break;
            }
        }
    }
}