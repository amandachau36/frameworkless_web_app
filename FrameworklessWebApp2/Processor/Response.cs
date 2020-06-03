using System.Net;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Processor
{
    public static class Response
    {
        public static void Send(HttpStatusCode statusCode, string message, HttpListenerResponse response)
        {
            SetStatusCode(response, statusCode);
            
            Headers.AddAllHeaders(response);
            
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);    //A buyes limited resource.   
                                                                                //convert string to byte array, body in the response
                                                                                
            response.ContentLength64 = buffer.Length;                    //it must be set explicitly before writing the the Return Stream Object  

            var output = response.OutputStream; 
            
            output.Write(buffer, 0, buffer.Length);  // forces send of response  

            output.Flush();        // clears all buffers for this stream and causes any buffered data to be written to the underlying device
                                   // flush forces incomplete blocks down.
            output.Close();        //Close the stream
                                    // Close() is required, unless we use "using" but thi may have consequences - resource management is important.
        }

        public static string PrepareMessage(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private static void SetStatusCode(HttpListenerResponse response, HttpStatusCode statusCode)
        {
            response.StatusCode = (int) statusCode;
        }
            
       
    }
}


// Thread.Sleep(1000);
//             
// Send(message, context);