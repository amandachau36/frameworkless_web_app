
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FrameworklessWebApp2.DataAccess;
using FrameworklessWebApp2.Models;


namespace FrameworklessWebApp2.Controllers
{
 
    public class UsersController : IController<User>  //UsersController<T>
    {
        private readonly DataManager _dataManager;

        public UsersController(DataManager dataManager)   
        {
            _dataManager = dataManager;
        }

        public User Post(IModel model) //TODO: could be context instead of IMODel  
        { 
            var user = (User) model;
            ValidateNewUser(user);
            return _dataManager.CreateUser(user); //Controller 
        }

        public List<User> Get()
        {
            return _dataManager.ReadUsers();
        }

        public User Get(int id)
        {
            return _dataManager.ReadUser(id);
        }

        public User Put(IModel model, int id)
        {
            return _dataManager.UpdateUser(id, (User)model);
        }

        public void Delete(int id)
        {
            _dataManager.DeleteUser(id);
        }

        private void ValidateNewUser(User user)
        {
            if(user.Name == null || user.Username == null|| user.Location == null)
            {
                throw new InvalidOperationsException("Missing name, username or location. ");
            }
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