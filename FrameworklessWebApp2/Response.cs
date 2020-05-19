using System.Net;

namespace FrameworklessWebApp2
{
    public static class Response
    {
        public static void Send(string message, HttpListenerContext context)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(message); // convert string to byte array, body in the response
            context.Response.ContentLength64 = buffer.Length;  
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);  // forces send of response  
            
            //context.Response.StatusCode 
            // TODO: do I need to close this? 
        }
       
    }
}