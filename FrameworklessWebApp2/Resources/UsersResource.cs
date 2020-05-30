using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Resources
{
    public class UsersResource : IResource
    {
        private readonly DataManager _dataManager;

        public UsersResource(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public string Get()
        {
           return _dataManager.GetUsers();
           
        }

        public string Get(int? id)
        {
            throw new System.NotImplementedException();
        }

        public void Put() 
        {
            //unused
        }

        public string Post(HttpListenerContext context)
        {
            var body = context.Request.InputStream;  //Controller
                            
            var reader = new StreamReader(body, context.Request.ContentEncoding);

            var json = reader.ReadToEnd();
                            
            var user = JsonConvert.DeserializeObject<User>(json);
                            
            var newUserList = _dataManager.AddUser(user); //Controller 
            
            _dataManager.WriteToTextFile(newUserList); //Controller

            return _dataManager.GetUsers();
        }

        public void Delete()
        {
            //throw new System.NotImplementedException(); //unused //TODO: breaks solid principle (interface segregation) 
        }
    }
}