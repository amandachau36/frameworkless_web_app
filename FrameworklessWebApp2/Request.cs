using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public class Request //TODO: processing of the request is the controller 
    {//TODO: think about wrapping the HTTPListenerContext
        
        private readonly DataManager _dataManager;
        public Request(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
       
        
        public void Process(HttpListenerContext context) //TODO: breakdown/ be able to use a new route
        {
            var request = context.Request; 
            var response = context.Response;
           
            switch (request.Url.AbsolutePath) 
            {
                case "/":
                    response.StatusCode = (int) HttpStatusCode.OK;  
                    Response.Send("Welcome to the Home Page", context);
                    break;
                case "/users": //Routing TODO: stragedy pattern
                    switch (request.HttpMethod)
                    {
                        case "GET": //Routing  
                            Console.WriteLine("Get users");
                            response.StatusCode = (int) HttpStatusCode.OK;  
                            //TODO: return json 
                            var htmlMessage = Html.Wrap("All Users", "<h1></h1>"); //VIEW //But this should probably be done in the front end?
                            Response.Send(htmlMessage, context); //TODO: send JSON  
                            break;
                        case "POST": 
                            Console.WriteLine("posting to /Users");
                            var body = request.InputStream;  //Controller
                            
                            var reader = new StreamReader(body, request.ContentEncoding);

                            var json = reader.ReadToEnd();
                            
                            var user = JsonConvert.DeserializeObject<User>(json);
                            
                            _dataManager.AddUser(user); //Controller 
                            _dataManager.WriteToTextFile(_dataManager.Users); //Controller
                            
                            response.StatusCode = (int) HttpStatusCode.Created; //view
                            
                            Console.WriteLine("============\n" + user.Name + $"({user.Username})");  //TODO: make logging better - Serilog outputs a structured log
                            Response.Send(user.Name, context); //View  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay may need to make not static 
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

//TODO: tests