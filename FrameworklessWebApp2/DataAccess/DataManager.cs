using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace FrameworklessWebApp2.DataAccess
{
    public class DataManager
    {
      
        private readonly IDatabase _database;

        public DataManager( IDatabase database)
        {
            _database = database;
        }
        
        public User CreateUser(User user)
        {
            var users = _database.Read();
            
            var id = users.Last().Id + 1;  
            user.SetId(id);
            
            users.Add(user);
            
            _database.Write(users);

            return user;
        }
        
        public List<User> ReadUsers()
        {
            var allUsers = _database.Read();

            return allUsers.Where(x => x.IsDeleted == false).ToList();
        }

        public User ReadUser(int id)
        {
            var users = ReadUsers();

            var index = users.FindIndex(x => x.Id == id);
            
            if (index < 0) 
                throw new HttpRequestException("Page not found: ", HttpStatusCode.NotFound); //TODO: probably need to throw a non HTTP  exception here then catch it in the web layer and throw a HTTP excpetion there 

            return users[index];

        }

        public User UpdateUser(int id, User user)
        {
            var users = _database.Read();

            var index = users.FindIndex(x => x.Id == id && x.IsDeleted == false);

            if (index < 0) 
                throw new HttpRequestException("Page not found: ", HttpStatusCode.NotFound);

            var propertiesToUpdate = user.GetType().GetProperties();

            foreach (var prop in propertiesToUpdate)
            {
                var value = prop.GetValue(user);

                if (value is null || prop.Name == "Id") continue;  //TODO: Throw Exception when trying to change ID 

                users[index].GetType().GetProperty(prop.Name)?.SetValue(users[index], prop.GetValue(user));
                
            }
            
            _database.Write(users);

            return users[index];

        }
        
        public void DeleteUser(int id) 
        {
            var users = ReadUsers();

            var index = users.FindIndex(x => x.Id == id);

            if (index < 0) 
                throw new HttpRequestException("Page not found: ", HttpStatusCode.NotFound);
            
            users[index].SetIsDeletedToTrue();
            
            _database.Write(users);
            
        }

        // private void WriteToTextFile(List<User> users)
        // {
        //     
        //     var usersList = new JArray(
        //         from u in users
        //         select new JObject(
        //             new JProperty("username", u.Username),
        //             new JProperty("name", u.Name),
        //             new JProperty("location", u.Location),
        //             new JProperty("id", u.Id),
        //             new JProperty("isDeleted", u.IsDeleted)
        //         )
        //     );
        //
        //     var sw = new StreamWriter(_path);
        //     
        //     sw.WriteLine(usersList);
        //     
        //     sw.Flush();
        //
        //     sw.Close();

        //}

   
    }

  
}


// private List<User> _users;
// public ReadOnlyCollection<User> Users => _users.AsReadOnly();


//https://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
//AppDomain.CurrentDomain.BaseDirectory, //restarting users over again , don't want to be in bin folder 
///Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/bin/Debug/netcoreapp3.1/DataAccess/Users.json