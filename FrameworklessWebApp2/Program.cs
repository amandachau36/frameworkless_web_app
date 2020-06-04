using System;
using System.IO;
using Serilog;
using Serilog.Formatting.Json;

namespace FrameworklessWebApp2
{
    class Program
    {
        static void Main(string[] prefixes)     //Uniform resource Identifer (URI) prefixes
        {
            var path = 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File( Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            var server = new Server();
            
            server.StartServer();
            
            Log.CloseAndFlush(); 
        }
    }
} 


            // if (!HttpListener.IsSupported)
            // {
            //     Console.WriteLine("HttpListener is not supported");
            // }
            //
            //
            // if (prefixes == null || prefixes.Length == 0)
            //     throw new AggregateException("prefixes missing");
            //     
            // var server = new HttpListener();
            //
            // foreach (var prefix in prefixes)
            // {
            //     server.Prefixes.Add(prefix);
            // }
            //
            // server.Start();                         //Allows this instance to receive incoming requests
            //
            // Console.WriteLine("Listening");
            //
            //                                         //The getContext method BLOCKS while waiting for a request
            // var context = server.GetContext(); 
            //
            // var request = context.Request;          // an object that represents a client request 
            //
            //                                         // obtain a response object
            // var response = context.Response;        //Object used to send a response back to the client
            //
            //
            // var responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            //
            // var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //
            // response.ContentLength64 = buffer.Length;     //Gets or sets the number of bytes in the body data included in the reponse
            //                                              //it must be set explicitly before writing the the Return Stream Object 
            //
            // var output = response.OutputStream;    //Gets a stream object to which a response can be written  
            //
            // output.Write(buffer, 0, buffer.Length);
            //
            // output.Close();                            //You must close the output stream 
            //
            // server.Stop();

            
            // //TODO: Asynchronous - multiple requests