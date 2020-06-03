
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Controllers
{
    public class UsersController : IController<User>  //UsersController<T>
    {
        private readonly DataManager _dataManager;

        public UsersController(DataManager dataManager)  
        {
            _dataManager = dataManager;
        }

        public User Post(User model)
        {
              return _dataManager.CreateUser(model); //Controller 
        }

        public List<User> Get()
        {
            return _dataManager.ReadUsers();
            //TODO: set status code here?
        }

        public User Get(int id)
        {
            return _dataManager.ReadUser(id);
        }

        public User Put(User model, int id)
        {
            return _dataManager.UpdateUser(id, model);
        }

        public void Delete(int id)
        {
            _dataManager.DeleteUser(id);
        }
    }
}


// RequestProcessor -> Controller -> Engine (Iresponse -> Status Code)