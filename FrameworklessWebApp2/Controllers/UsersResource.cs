
using System.Net;
using FrameworklessWebApp2.DataAccess;

using Newtonsoft.Json;

namespace FrameworklessWebApp2.Controllers
{
    // public class UsersResource : IResource
    // {
    //     private readonly DataManager _dataManager;
    //
    //     public UsersResource(DataManager dataManager)
    //     {
    //         _dataManager = dataManager;
    //     }
    //     
    //     public string Get()
    //     {
    //        return _dataManager.ReadUsers();
    //        
    //     }
    //     
    //
    //     public string Put(HttpListenerContext context) 
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public string Post(HttpListenerContext context)
    //     {
    //         var json = Json.Read(context); 
    //         
    //         var user = JsonConvert.DeserializeObject<User>(json);
    //         
    //         _dataManager.CreateUser(user); //Controller 
    //
    //         return _dataManager.ReadUsers();
    //     }
    //
    //     public void Delete()
    //     {
    //         //throw new System.NotImplementedException(); //unused //TODO: breaks solid principle (interface segregation) 
    //     }
    // }
}

//TODO: test what would happen with ID