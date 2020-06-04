
using System.Collections.Generic;
using FrameworklessWebApp2.DataAccess;


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
            //TODO: set status code here? But would either need to send back the statuscode or send response 
        }

        public User Get(int id)
        {
            return _dataManager.ReadUser(id);
        }

        public User Put(User model, int id)
        {
            //TODO:
            return _dataManager.UpdateUser(id, model);
        }

        public void Delete(int id)
        {
            _dataManager.DeleteUser(id);
        }
    }
}

// public class SomeController : IController<object>
// {
//     private IController<User> _inject;
//
//     public object Post(object model)
//     {
//         return _inject.Post((User)model);
//     }
//
//     public List<User> Get()
//     {
//         return _inject.Get();
//     }
//
//     public User Get(int id)
//     {
//         return _inject.Get(id);
//     }
//
//     public User Put(User model, int id)
//     {
//         return _inject.Put(model, id);
//     }
//
//     public void Delete(int id)
//     {
//         _inject.Delete(id);
//     }
// }
// RequestProcessor -> Controller -> Engine (Iresponse -> Status Code)