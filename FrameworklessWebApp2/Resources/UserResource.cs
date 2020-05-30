using System;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Resources
{
    public class UserResource : IResource
    {
        private readonly DataManager _dataManager;

        public UserResource(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public string Get()
        {
            throw new System.NotImplementedException(); // unused
        }

        public string Get(int? id)
        {
           return _dataManager.ReadUser(id);
        }

        public string Put(int? id, HttpListenerContext context)
        {
            var body = context.Request.InputStream;  //Controller
                            
            var reader = new StreamReader(body, context.Request.ContentEncoding);

            var json = reader.ReadToEnd();
                            
            var userUpdate = JsonConvert.DeserializeObject<User>(json);

            var updatedUsersList = _dataManager.UpdateUser(id, userUpdate);
            
            _dataManager.WriteToTextFile(updatedUsersList);

            return _dataManager.ReadUser(id);
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