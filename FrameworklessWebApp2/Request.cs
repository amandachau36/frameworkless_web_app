using System;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Resources;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public class Request //TODO: processing of the request is the controller 
    {//TODO: think about wrapping the HTTPListenerContext
        
        private readonly DataManager _dataManager;
        private readonly IResource _users;

        public Request(DataManager dataManager, IResource resource)
        {
            _dataManager = dataManager;
            _users = resource;
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
                            var getMessage = _users.Get(); //Controller
                            response.StatusCode = (int) HttpStatusCode.OK;
                            Response.Send(getMessage, context); 
                            break;
                        case "POST": 
                            Console.WriteLine("posting to /Users");

                            var postMessage = _users.Post(context);
                            
                            response.StatusCode = (int) HttpStatusCode.Created; //view
                            //Console.WriteLine("============\n" + user.Name + $"({user.Username})");  //TODO: make logging better - Serilog outputs a structured log
                            Response.Send(postMessage, context); //View  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay may need to make not static 
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

//var htmlMessage = Html.Wrap("All Users", "<h1></h1>"); //VIEW //But this should probably be done in the front end?