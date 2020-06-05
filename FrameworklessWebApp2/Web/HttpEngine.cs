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

        public ResponseMessage Process(HttpListenerContext context) //TODO: breakdown/ be able to use a new route
        {
            var request = context.Request; 
            var response = context.Response;
            ResponseMessage responseMessage;

            try
            {
                var uriSegments = RequestProcessor.GetProcessedUriSegments(request.Url);
                dynamic controller = RequestProcessor.GetController(uriSegments[1], _dataManager);
                var id = RequestProcessor.GetId(uriSegments);
                var verb = RequestProcessor.GetVerb(request.HttpMethod);
                _logger.Debug($"uriSegments[1]: {uriSegments[1]}, id: {id}, verb: {verb}");

                switch (verb)
                {
                    case HttpVerb.Get: //Routing // URL  
                        var getMessage = id == null
                            ? controller.Get()
                            : controller.Get(id.GetValueOrDefault());
                        _logger.Debug($"Sending get response. Message: {getMessage}");
                        return new ResponseMessage(HttpStatusCode.OK, getMessage);
                    case HttpVerb.Put: //URL and body
                        var modelToUpdate = RequestProcessor.GetModel(uriSegments[1], request);
                        var updatedUser = controller.Put(modelToUpdate, id.GetValueOrDefault());
                        _logger.Debug($"Sending put response. Message: {updatedUser}");
                        return new ResponseMessage(HttpStatusCode.OK, updatedUser);
                    case HttpVerb.Post: //body
                         var modelToCreate = RequestProcessor.GetModel(uriSegments[1], request);
                         var newUser = controller.Post(modelToCreate);
                         _logger.Debug($"Sending post response. Message: {newUser}");
                         return new ResponseMessage(HttpStatusCode.Created, newUser);
                    case HttpVerb.Delete: //URL
                         controller.Delete(id.GetValueOrDefault());
                         _logger.Debug($"Sending delete response. Message: Deleted {id}");
                         return new ResponseMessage(HttpStatusCode.OK, $"Deleted {id}");
                    default:
                        throw new HttpRequestException($"Invalid http method: {verb} for ",
                            HttpStatusCode.MethodNotAllowed);
                }
            }
            catch (HttpRequestException e)
            {
                _logger.Error(
                    $"Exception message: {e.Message + request.Url}, Status Code: {(int) e.StatusCode} {e.StatusCode}");
                Response.Send(e.StatusCode, e.Message + request.Url, response);
            }
            catch (Exception e)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                _logger.Error($"Exception message: {e.Message}, Status Code: {HttpStatusCode.InternalServerError}");
                Response.Send(HttpStatusCode.InternalServerError, e.Message, response);
            }
           
            throw new Exception("it should never reach here");

        }

        public void Send(ResponseMessage responseMessage, HttpListenerResponse response)
        {
            var message = Response.PrepareMessage(responseMessage.Message);
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
