using System;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Resources
{
    public class UserResource : IResource
    {
        private readonly int _id;
        private readonly DataManager _dataManager;

        public UserResource(DataManager dataManager, int id)
        {
            _dataManager = dataManager;
            _id = id;
        }
        public string Get()
        {
            return _dataManager.ReadUser(_id);
        }
        
        public string Put(HttpListenerContext context)
        {
            
            var json = Json.Read(context);
                            
            var userUpdate = JsonConvert.DeserializeObject<User>(json);
            
            _dataManager.UpdateUser(_id, userUpdate);
        
            return _dataManager.ReadUser(_id);
        }

        public string Post(HttpListenerContext context) //unused
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}