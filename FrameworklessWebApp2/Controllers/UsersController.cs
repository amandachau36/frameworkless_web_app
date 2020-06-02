using System;
using System.IO;
using System.Net;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Resources;
using Newtonsoft.Json;

namespace FrameworklessWebApp2.Controllers
{
    public class UsersController 
    {
     
        private readonly DataManager _dataManager;

        public UsersController(DataManager dataManager)  //TODO: Controller - not to deal with context, process before, the controller should receive an object/model/user
        {
            _dataManager = dataManager;
        }
        
        
        public string Post(HttpListenerContext context)
        {
            var json = Json.Read(context); 
            
            var user = JsonConvert.DeserializeObject<User>(json);
            
            _dataManager.CreateUser(user); //Controller 

            return _dataManager.ReadUsers();
        }
        
        public string Get()
        {
            return _dataManager.ReadUsers();
           
        }
        
        public string Get(int id)
        {
            return _dataManager.ReadUser(id);
        }
        
        public string Put(HttpListenerContext context, int id) //TODO: consider passing in model instead of entire HH
        {
            
            var json = Json.Read(context);
                            
            var userUpdate = JsonConvert.DeserializeObject<User>(json);
            
            _dataManager.UpdateUser(id, userUpdate);
        
            return _dataManager.ReadUser(id);
        }
        

        public void Delete()
        {
            throw new System.NotImplementedException();
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