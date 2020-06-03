
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Controllers
{
    public class UsersController<T> : IController<User>
    {
        private readonly DataManager _dataManager;

        public UsersController(DataManager dataManager)  //TODO: Controller - not to deal with context, process before, the controller should receive an object/model/user
        {
            _dataManager = dataManager;
        }

        public User Post(User model)
        {
              _dataManager.CreateUser(model); //Controller 
            
              return _dataManager.ReadUsers().Last();
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
            _dataManager.UpdateUser(id, model);
            
            return _dataManager.ReadUser(id); //TODO: return updated object without reading
        }

        public void Delete(int id)
        {
            _dataManager.DeleteUser(id);
        }
    }
}

// public UsersController()
// { 
// UserRepository _userRepository
// UsersController(UserRepository userRepository)
// {
//     _userRepository = userRepository;
// }
//     
// [GET]
// public User Get(Guid id)
// {
//     return _userRepository.GetById(id);
// }
//     
//     
// [PUT]
// public User Update(User user)
// {
//     return _userRepository.Update(user);
// }
//     
// [GET]
// public List<Users> Get()
// {
//     return _userRepository.Get();
// }
//     
// [POST]
// public User Create(User)
// {
//     return _userRepository.Create(user);
// }
// }
//
// RequestProcessor -> Controller -> Engine (Iresponse -> Status Code)