
using System.Collections.Generic;
using FrameworklessWebApp2.DataAccess;

namespace FrameworklessWebApp2.Resources
{
    public class Users : IResource
    {
        private readonly DataManager _dataManager;

        public Users(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public List<User> Get()
        {
           return _dataManager.GetUsers();
           
        }

        public void Put() 
        {
            throw new System.NotImplementedException(); //unused
        }

        public void Post()
        {
            
        }

        public void Delete()
        {
            throw new System.NotImplementedException(); //unused //TODO: breaks solid principle (interface segregation) 
        }
    }
}