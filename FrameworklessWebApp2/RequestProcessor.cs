using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using FrameworklessWebApp2.Controllers;
using FrameworklessWebApp2.DataAccess;


namespace FrameworklessWebApp2
{
    public class RequestProcessor //TODO: processing of the request is the controller 
    {//TODO: think about wrapping the HTTPListenerContext

        private readonly DataManager _dataManager;
        public RequestProcessor(DataManager dataManager)
        {
            _dataManager = dataManager;
           
        }

        public void Process(HttpListenerContext context) //TODO: breakdown/ be able to use a new route
        {
            
            var request = context.Request; 
            var response = context.Response;
            try
            {
                
                var verb = ConvertHttpMethodToEnum(request.HttpMethod);
            
                var resourceInfo = ConvertPathToResource(request.Url);

                //var resource = ResourceFactory.CreateResource(resourceInfo, _dataManager);   //Routing  //
                var usersController = resourceInfo.Item1 switch
                {
                    Controller.Users => new UsersController(_dataManager),
                    _ => throw new HttpRequestException("Page not found: ")
                };

                var id = resourceInfo.Item2.GetValueOrDefault();
                
                
                
                switch (verb)
                {
                    case HttpVerb.Get: //Routing // URL
                        Console.WriteLine("hello from get"); //TODO: make logging better - Serilog outputs a structured log
                        string getMessage;
                        getMessage = resourceInfo.Item2 == null ? usersController.Get() : usersController.Get(id);
                        response.StatusCode = (int) HttpStatusCode.OK;
                        Response.Send(getMessage, context); //TODO: NOT CONTROLLER, return json/string  
                        break;
                    case HttpVerb.Put:  //URL and body
                        var putMessage = usersController.Put(context, id);
                        response.StatusCode = (int) HttpStatusCode.OK;
                        Response.Send(putMessage, context);
                        break;
                    case HttpVerb.Post:  //body
                        var postMessage = usersController.Post(context); // Controller
                        response.StatusCode = (int) HttpStatusCode.Created; //view
                        Response.Send(postMessage, context); //View  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay may need to make not static 
                        break;
                    case HttpVerb.Delete: //URL
                        break;
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
            
            throw new HttpRequestException("Invalid http method: " + httpMethod);
        }

        private (Controller, int? id) ConvertPathToResource(Uri uri)
        {
            var segments = uri.Segments;

            var processedSegments = segments.Select(s => s.TrimEnd('/')).ToList();

            var controller = GetController(processedSegments[1]);

            var id = GetId(processedSegments);

            return (controller, id);
        }

        private static Controller GetController(string controllerString)
        {

            if (Enum.TryParse(controllerString, true, out Controller controller))
            {
                return controller;
            }
            
            throw new HttpRequestException($"Controller not found: {controllerString}");
        }

        private static int? GetId(List<string> processedSegments)
        {
            if (processedSegments.Count > 2 && int.TryParse(processedSegments[2], out var id))
                return id;

            return null; //TODO: case users/cats Throw exception 

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
