using System.Net;

namespace FrameworklessWebApp2
{
    public static class Response
    {
        public static void Send(string message, HttpListenerContext context)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);    //TODO: is buffer just temp storage?  
                                                                                //convert string to byte array, body in the response
            context.Response.ContentLength64 = buffer.Length;                    //it must be set explicitly before writing the the Return Stream Object  

            var output = context.Response.OutputStream; 
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);  // forces send of response  
            
            output.Flush();        // clears all buffers for this stream and causes any buffered data to be written to the underlying device
                                   // flush forces incomplete blocks down.
            output.Close();        //Close the stream
                                    // Close() is required, unless we use "using" but thi may have consequences - resource management is important. 
        }
       
    }
}