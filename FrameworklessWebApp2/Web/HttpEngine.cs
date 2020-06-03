using System;
using System.Net;
using System.Net.Http;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Processor;


namespace FrameworklessWebApp2
{
    public class HttpEngine 
    {

        private readonly DataManager _dataManager;
        public HttpEngine(DataManager dataManager)
        {
            _dataManager = dataManager;
           
        }

        public void Process(HttpListenerContext context) //TODO: breakdown/ be able to use a new route
        {
            var request = context.Request; 
            var response = context.Response;
            try
            {
                var uriSegments = RequestProcessor.GetProcessedUriSegments(request.Url);

                dynamic controller = RequestProcessor.GetController(uriSegments[1], _dataManager);
                var id = RequestProcessor.GetId(uriSegments);
                var verb = RequestProcessor.GetVerb(request.HttpMethod);
                
                switch (verb)
                {
                    case HttpVerb.Get: //Routing // URL  //TODO: make logging better - Serilog outputs a structured log
                        var getMessage = id == null 
                            ? Response.PrepareMessage(controller.Get()) 
                            : Response.PrepareMessage(controller.Get(id.GetValueOrDefault()));
                        Response.Send( HttpStatusCode.OK, getMessage, response); 
                        break;
                    case HttpVerb.Put:  //URL and body
                        var modelToUpdate = RequestProcessor.GetModel(uriSegments[1], request);
                        var updatedUser = controller.Put(modelToUpdate, id.GetValueOrDefault());
                        var putMessage = Response.PrepareMessage(updatedUser);
                        Response.Send( HttpStatusCode.OK, putMessage, response);
                        break;
                    case HttpVerb.Post:  //body
                        var modelToCreate = RequestProcessor.GetModel(uriSegments[1], request);
                        var newUser = controller.Post(modelToCreate);
                        var postMessage = Response.PrepareMessage(newUser);
                        Response.Send(HttpStatusCode.Created, postMessage,response); //View  // Must send response but sometimes if doesn't have content 204 /TODO Idisplay may need to make not static 
                        break;
                    case HttpVerb.Delete: //URL
                        controller.Delete(id.GetValueOrDefault());
                        Response.Send(HttpStatusCode.OK, "Deleted " + id , response);
                        break;
                    default:
                        throw new HttpRequestException("Page not found: "); 
                }
            }
            catch (HttpRequestException e) //TODO: custom exception with a property? 
            {
                Response.Send(HttpStatusCode.NotFound,e.Message + request.Url, response);
            }
            catch (Exception e)
            {
                Response.Send(HttpStatusCode.InternalServerError, e.Message, response);
            }
        }
    }
}

    
//GET ROUTE
//GET CONTROLLER
//GET VERB
//GET MODEL (convert body to model)



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
// routes.Add(Tuple.Create(POST, "/..."), todi);
// routes.Add((POST, "..."), resource)
// /// ...
// resourceBuilder.Build(routes);
// // customer resource builder
// void Build(routes)
// {
// routes.Add(...)
// }
