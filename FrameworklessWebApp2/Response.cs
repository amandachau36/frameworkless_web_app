using System.Net;
using System.Threading;

namespace FrameworklessWebApp2
{
    public static class Response
    {
        public static void Send(string message, HttpListenerContext context)
        {
            message = $"<h1>{message}</h1>";
            
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);    //A buyes limited resource.   
                                                                                //convert string to byte array, body in the response
            
            Headers.AddAllHeaders(context.Response);
            
            context.Response.ContentLength64 = buffer.Length;                    //it must be set explicitly before writing the the Return Stream Object  

            var output = context.Response.OutputStream; 
            
            output.Write(buffer, 0, buffer.Length);  // forces send of response  

            output.Flush();        // clears all buffers for this stream and causes any buffered data to be written to the underlying device
                                   // flush forces incomplete blocks down.
            output.Close();        //Close the stream
                                    // Close() is required, unless we use "using" but thi may have consequences - resource management is important.
        }
       
    }
}


// Thread.Sleep(1000);
//             
// Send(message, context);