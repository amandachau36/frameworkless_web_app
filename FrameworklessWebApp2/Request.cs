using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Resources;


namespace FrameworklessWebApp2
{
    public class Request //TODO: processing of the request is the controller 
    {//TODO: think about wrapping the HTTPListenerContext
        
        private readonly Dictionary<Resource, IResource> _resources;

        public Request(DataManager dataManager, Dictionary<Resource, IResource> resources)
        {
            _resources = resources;
        }
     
        
        public void Process(HttpListenerContext context) //TODO: breakdown/ be able to use a new route
        {
            var request = context.Request; 
            var response = context.Response;

            var path = ConvertPathToResource(request.Url);

            var verb = ConvertHttpMethodToEnum(request.HttpMethod);

            try
            {
                switch (path.Item1)
                {
                    case Resource.Users: //Routing TODO: strategy pattern
                    switch (verb)
                    {
                        case HttpVerb.Get: //Routing  
                            Console.WriteLine("Get users");  //TODO: make logging better - Serilog outputs a structured log
                            var getMessage = _resources[Resource.Users].Get(); //Controller
                            response.StatusCode = (int) HttpStatusCode.OK;
                            Response.Send(getMessage, context); 
                            break;
                        
                        case HttpVerb.Post: 
                            Console.WriteLine("posting to /Users");
                            var postMessage = _resources[Resource.Users].Post(context); // Controller
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
                            var getMessage = _resources[Resource.User].Get(path.Item2);
                            response.StatusCode = (int) HttpStatusCode.OK;
                            Response.Send(getMessage, context);
                            break;
                        case HttpVerb.Put:
                            var putMessage = _resources[Resource.User].Put(path.Item2, context);
                            response.StatusCode = (int) HttpStatusCode.OK;
                            Response.Send(putMessage, context);
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
                    throw new HttpRequestException("Page not found: ");
                }

            }
            catch (HttpRequestException e)
            {
                response.StatusCode = (int) HttpStatusCode.NotFound;  
                Response.Send(e.Message + request.Url, context);  
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
