using System.Net;
using FrameworklessWebApp2.DataAccess;

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
            throw new System.NotImplementedException();
        }

        public string Get(int? id)
        {
           return _dataManager.GetUser(id);
        }

        public void Put()
        {
            throw new System.NotImplementedException();
        }

        public string Post(HttpListenerContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}