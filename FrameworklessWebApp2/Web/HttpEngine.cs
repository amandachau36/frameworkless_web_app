using System;
using System.Net;
using System.Net.Http;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Web.HttpRequest;
using FrameworklessWebApp2.Web.HttpResponse;
using Serilog;


namespace FrameworklessWebApp2.Web
{
    public class HttpEngine 
    {
        private readonly DataManager _dataManager;
        private readonly ILogger _logger;

        public HttpEngine(DataManager dataManager, ILogger logger)
        {
            _dataManager = dataManager; 
            _logger = logger;
        }

        public ResponseMessage Process(IHttpListenerRequestWrapper request) //TODO: breakdown/ be able to use a new route
        {
            try
            {
              
                var uriSegments = RequestProcessor.GetProcessedUriSegments(request.Uri);
                dynamic controller = RequestProcessor.GetController(uriSegments[1], _dataManager);
                var id = RequestProcessor.GetId(uriSegments);
                var verb = RequestProcessor.GetVerb(request.HttpMethod);
                _logger.Debug($"controller/model: {uriSegments[1]}, id: {id}, verb: {verb}");

                switch (verb)
                {
                    case HttpVerb.Get: //Routing // URL  
                        var getMessage = id == null
                            ? controller.Get()
                            : controller.Get(id.GetValueOrDefault());
                        _logger.Information("Preparing get message");
                        return new ResponseMessage(HttpStatusCode.OK, getMessage);
                    case HttpVerb.Put: //URL and body
                        var modelToUpdate = RequestProcessor.GetModel(uriSegments[1], request);
                        var updatedUser = controller.Put(modelToUpdate, id.GetValueOrDefault());
                        _logger.Information("Preparing put message");
                        return new ResponseMessage(HttpStatusCode.OK, updatedUser);
                    case HttpVerb.Post: //body
                         var modelToCreate = RequestProcessor.GetModel(uriSegments[1], request);
                         var newUser = controller.Post(modelToCreate);
                         _logger.Information("Preparing post message");
                         return new ResponseMessage(HttpStatusCode.Created, newUser);
                    case HttpVerb.Delete: //URL
                         controller.Delete(id.GetValueOrDefault());
                         _logger.Information("Preparing delete message");
                         return new ResponseMessage(HttpStatusCode.OK, $"Deleted {id}");
                    default:
                        throw new HttpRequestException($"Invalid http method: {verb} for ",
                            HttpStatusCode.MethodNotAllowed);
                }
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Exception message: {e.Message + request.Uri}, Status Code: {(int) e.StatusCode} {e.StatusCode}");
                return new ResponseMessage(e.StatusCode, e.Message + request.Uri);
            }
            catch (Exception e)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                _logger.Error($"Exception message: {e.Message}, Status Code: {HttpStatusCode.InternalServerError}");
                return new ResponseMessage(statusCode, e.Message);
            }
            

        }

        public void Send(ResponseMessage responseMessage, HttpListenerResponse response)
        {
            var message = Response.PrepareMessage(responseMessage.Message);
            _logger.Debug($"Sending response. \nStatusCode: {responseMessage.StatusCode}. \nMessage: {message}"); //TODO: does this log errors twice?
            Response.Send(responseMessage.StatusCode, message, response );
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
