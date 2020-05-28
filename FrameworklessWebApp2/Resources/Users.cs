using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Resources
{
    public class Users : IResource
    {
        private readonly DataManager _dataManager;

        public Users(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public string Get()
        {
           return _dataManager.GetUsers();
           
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