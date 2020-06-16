using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using FluentAssertions;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Web;
using FrameworklessWebApp2.Web.HttpResponse;
using Serilog;
using Serilog.Events;
using Xunit;

namespace FrameworklessWebApp2.Tests
{
    public class HttpEngineUnitTests
    {
        [Theory]
        [MemberData(nameof(GetRequests))]
        public void It_Should_Return_Correct_ResponseMessage_Given_A_Request(Uri uri, string httpMethod, string body, ResponseMessage expectedResponseMessage)
        {
            //arrange
            var allUsers = Users.CreateTestUsers();

            var loggerStub = new LoggerStub();

            var mockDatabase = new MockDatabase(allUsers);

            var httpEngine = new HttpEngine(new DataManager(mockDatabase), loggerStub);
            
            var repo =  new MemoryStream(Encoding.UTF8.GetBytes(body));
            var contentEncoding = Encoding.UTF8;
            
            var request = new HttpListenerRequestStub(uri, httpMethod, repo, contentEncoding);
            
            //act
            var responseMessage = httpEngine.Process(request);
            
            //assert
            responseMessage.Should().BeEquivalentTo(expectedResponseMessage);
            
        }
        
        
        public static IEnumerable<object[]> GetRequests()
        {
            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "GET",
                "",
                new ResponseMessage(HttpStatusCode.OK, Users.CreateTestUsers()),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/u"),
                "GET",
                "",
                new ResponseMessage(HttpStatusCode.NotFound, "Page not found: http://localhost:8080/u"),
            };

            yield return new object[]
            {
                new Uri("http://localhost:8080/users/1"),
                "GET",
                "",
                new ResponseMessage(HttpStatusCode.OK, Users.CreateTestUsers()[0]),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users/1000"),
                "GET",
                "",
                new ResponseMessage(HttpStatusCode.NotFound, "Page not found: http://localhost:8080/users/1000"),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "POST",
                "{\"username\": \"D\", \"name\": \"Dan\", \"location\": \"Denmark\"}",
                new ResponseMessage(HttpStatusCode.Created, Users.CreateTestUser(new User {Name = "Dan", Username = "D", Location = "Denmark"}, 4)),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "POST",
                "{\"username\": \"D\", \"name\": \"Dan\", \"location\": \"Denmark\"}",
                new ResponseMessage(HttpStatusCode.Created, Users.CreateTestUser(new User {Name = "Dan", Username = "D", Location = "Denmark"}, 4)),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/cat"),
                "POST",
                "{\"username\": \"D\", \"name\": \"Dan\", \"location\": \"Denmark\"}",
                new ResponseMessage(HttpStatusCode.NotFound, "Page not found: http://localhost:8080/cat"),
            };


            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "POST",
                "{\"id\": 100, \"username\": \"D\", \"name\": \"Dan\", \"location\": \"Denmark\"}",
                new ResponseMessage(HttpStatusCode.Created, Users.CreateTestUser(new User {Name = "Dan", Username = "D", Location = "Denmark"}, 4)),
            };
            
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "POST",
                "{\"name\": \"Dan\", \"location\": \"Denmark\"}",
                new ResponseMessage(HttpStatusCode.BadRequest, "Missing name, username or location. http://localhost:8080/users")
            };
            
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "POST",
                "{\"isDeleted\": true, \"username\": \"D\", \"name\": \"Dan\", \"location\": \"Denmark\"}",
                new ResponseMessage(HttpStatusCode.Created, Users.CreateTestUser(new User {Name = "Dan", Username = "D", Location = "Denmark"}, 4)),
            };
         
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users/2"),
                "PUT",
                "{\"location\": \"Barbados\"}",
                new ResponseMessage(HttpStatusCode.OK, Users.CreateTestUser(new User {Name = "Bob", Username = "B", Location = "Barbados"}, 2))
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users/700"),
                "PUT",
                "{\"location\": \"Barbados\"}",
                new ResponseMessage(HttpStatusCode.NotFound, "Page not found: http://localhost:8080/users/700")
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users"),
                "PUT",
                "",
                new ResponseMessage(HttpStatusCode.NotFound, "Page not found for put request: http://localhost:8080/users"),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users/1"),
                "DELETE",
                "",
                new ResponseMessage(HttpStatusCode.OK, "Deleted users id:1"),
            };
            
            yield return new object[]
            {
                new Uri("http://localhost:8080/users/1000"),
                "DELETE",
                "",
                new ResponseMessage(HttpStatusCode.NotFound, "Page not found: http://localhost:8080/users/1000"),
            };

            
          
            


            // yield return new object[]
            // {
            //     new Uri("http://localhost:8080/cat"),
            //     "POST",
            //     "{\"username\": \"D\", \"name\": \"Dan\", \"location\": \"Denmark\"}",
            //     new ResponseMessage(HttpStatusCode.NotFound, "Page not found: http://localhost:8080/cat"),
            // };





        }
    }

    internal class Users
    {
        public static List<User> CreateTestUsers()
        {
            var user1 = new User {Name = "Ann", Username = "A", Location = "Australia"};
            user1.SetId(1);
            
            var user2 = new User {Name = "Bob", Username = "B", Location = "Bolivia"};
            user2.SetId(2);
            
            var user3 = new User {Name = "Chris", Username = "C", Location = "Canada"};
            user3.SetId(3);

            return new List<User>
            {
                user1,
                user2,
                user3
            };

        }

        public static User CreateTestUser(User user, int id)
        {
            
            user.SetId(id);
            
            return user;
        }
        
    }
    
    internal class LoggerStub : ILogger
    {
        public void Debug(string messageTemplate)
        {
                    
        }

        public void Error(string messageTemplate)
        {
                    
        }

        public void Information(string messageTemplate)
        {
                    
        }

        public void Write(LogEvent logEvent)
        {
                    
        }
    }
    internal class HttpListenerRequestStub : IHttpListenerRequestWrapper
    {
        public Uri Uri { get; }
        public string HttpMethod { get; }
        public Stream InputStream { get; }
        public Encoding ContentEncoding { get; }

        public HttpListenerRequestStub(Uri uri, string httpMethod, Stream inputStream, Encoding contentEncoding)
        {
            Uri = uri;
            HttpMethod = httpMethod;
            InputStream = inputStream;
            ContentEncoding = contentEncoding;
        }
    }

    internal class MockDatabase : IDatabase
    {
        private readonly List<User> _users;

        public MockDatabase(List<User> users)
        {
            _users = users;
        }
        public void Write(List<User> users)
        {
                    
        }

        public List<User> Read()
        {
            return _users;
        }
    }

}

        
//var path = Path.Combine(Directory.GetCurrentDirectory(), "TestDatabase", "TestUsers.json");
//var path =
// "/Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2.Tests/TestDatabase/TestUsers.json";
//Database.SeedTextFile(allUsers, path);
        
//act
//assert 
// public class A
// {
//     void B()
//     {
//         var httpEngine = new HttpEngine();
//         
//         httpEngine.Process(new HttpListenerContextWrapper(theOriginalContext));
//         
//         // in the test
//         httpEngine.Process(new StubHttpListontext()); // constructor with mock info 
//     }
// }
            
            
//https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistenercontext?view=netcore-3.1
//httpEngine.Process(new HttpListenerContext()); 
// var server = new Server();
// server.StartServer();
            
// var mock = new Mock<HttpListenerContext>();
// mock.Setup(x => x.Request.Url)
//     .Returns(new Uri("/users"));
// mock.Setup(x => x.Request.HttpMethod)
//     .Returns("get");

