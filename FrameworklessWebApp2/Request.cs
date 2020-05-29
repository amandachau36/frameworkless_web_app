using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Resources;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public class Request //TODO: processing of the request is the controller 
    {//TODO: think about wrapping the HTTPListenerContext
        
        private readonly IResource _users;

        public Request(DataManager dataManager, IResource resource)
        {
            _users = resource;
        }
     
        
        public void Process(HttpListenerContext context) //TODO: breakdown/ be able to use a new route
        {
            var request = context.Request; 
            var response = context.Response;

            var path = ConvertPathToResource(request.Url);

            var verb = ConvertHttpMethodToEnum(request.HttpMethod);

            switch (path.Item1)
            {
                // case "/":
                //     response.StatusCode = (int) HttpStatusCode.OK;  
                //     Response.Send("Welcome to the Home Page", context);
                //     break;
                case Resource.Users: //Routing TODO: strategy pattern
                    switch (verb)
                    {
                        case HttpVerb.Get: //Routing  
                            Console.WriteLine("Get users");  //TODO: make logging better - Serilog outputs a structured log
                            var getMessage = _users.Get(); //Controller
                            response.StatusCode = (int) HttpStatusCode.OK;
                            Response.Send(getMessage, context); 
                            break;
                        
                        case HttpVerb.Post: 
                            Console.WriteLine("posting to /Users");
                            var postMessage = _users.Post(context); // Controller
                            response.StatusCode = (int) HttpStatusCode.Created; //view
                            Response.Send(postMessage, context); //View  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay may need to make not static 
                            break;
                    }
                    break;
                case Resource.User:
                    switch (verb)
                    {
                        case HttpVerb.Get: //Routing  
                            Console.WriteLine($"/users/{path.Item2}");
                            break;
                        case HttpVerb.Put:
                            break;
                        case HttpVerb.Delete:
                            break; 
                    }
                    break;
                // case "/countries":
                //     response.StatusCode = (int) HttpStatusCode.OK;  
                //     Response.Send("All Countries", context);
                //     break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;  
                    Response.Send("Page not found: " + request.Url, context);  
                    break;
                
            }
        }

        private HttpVerb ConvertHttpMethodToEnum(string httpMethod)
        {
            if (Enum.TryParse(httpMethod, true, out HttpVerb verb))
            {
                return verb;
            }
            
            throw new InvalidEnumArgumentException("Invalid http method: " + httpMethod);
        }

        private (Resource, int? id) ConvertPathToResource(Uri uri)
        {
            var segments = uri.Segments;

            var processedSegments = segments.Select(s => s.TrimEnd('/')).ToList();

            Resource resource = Resource.ResourceNotFound;

            int? id = null;

            if (processedSegments[1].ToLower() == "users")
            {
                if (processedSegments.Count == 2)
                {
                    resource = Resource.Users;
                }

                if (processedSegments.Count == 3 && int.TryParse(processedSegments[2], out int num))
                {
                    resource = Resource.User;
                    id = num;
                }
            }
            
            // Console.WriteLine("---resource----" + resource);
            // Console.WriteLine("---id----" + id);
            

            return (resource, id);
        }
    }
}

//TODO: tests

//var htmlMessage = Html.Wrap("All Users", "<h1></h1>"); //VIEW //But this should probably be done in the front end?
// class Resource {
// public Response Process(Request request) ; // interface
// }
// var routes = new Dictionary<string, Resource>();
// routes.Add("/customers", new Resource.. ) // where is the verbs?
// routes.Add("/customers/:customerId")
// var routes = new Dictionary<Tuple<Verb, string>, Resource>();
// var routes1 = new Dictionary<(Verb, string), Resource>();
// routes.Add(Tuple.Create(POST, "/..."), todo);
// routes.Add((POST, "..."), resource)
// /// ...
// resourceBuilder.Build(routes);
// // customer resource builder
// void Build(routes)
// {
// routes.Add(...)
// }
