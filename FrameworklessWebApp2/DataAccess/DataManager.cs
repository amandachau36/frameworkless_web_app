using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrameworklessWebApp2.DataAccess
{
    public class DataManager
    {
        public void WriteToTextFile(List<User> users)
        {
            
            var usersList = new JArray(
                from u in users
                select new JObject(
                    new JProperty("username", u.Username),
                    new JProperty("name", u.Name),
                    new JProperty("location", u.Location),
                    new JProperty("id", u.Id)
                )
            );

            var sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "DataAccess", "Users.json"));
            
            sw.WriteLine(usersList);
            
            sw.Flush();

            sw.Close();

        }

        public List<User> CreateUser(User user)
        {
            var users = GetAllUsersList();
            
            var id = users.Last().Id + 1;                            //TODO: not ideal
            User.SetId(user, id);
            
            users.Add(user);
            
            return users;
        }
        public string ReadUsers() 
        {
            var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "DataAccess", "Users.json"));

            var json= sr.ReadToEnd();
            
            sr.Close();

            return json;
          
        }

        public string ReadUser(int? id)
        {
            var users = GetAllUsersList();

            var index = users.FindIndex(x => x.Id == id);
            
            if (index < 0) 
                throw new HttpRequestException("Page not found: ");

            return JsonConvert.SerializeObject(users[index]);

        }

        public List<User> UpdateUser(int? id, User user)
        {
            var users = GetAllUsersList();

            var index = users.FindIndex(x => x.Id == id);

            if (index < 0) 
                throw new HttpRequestException("Page not found: ");
            
            foreach (var prop in user.GetType().GetProperties())
            {
                var value = prop.GetValue(user);

                if (value is null || prop.Name == "Id") continue;
                
                users[index].GetType().GetProperty(prop.Name)?.SetValue(users[index], prop.GetValue(user));
                
                Console.WriteLine("prop" + prop.Name);
                Console.WriteLine("...." + prop.GetValue(user));
                // if (!string.IsNullOrWhiteSpace(value))
                // {
                //     prop.Name + prop.GetValue(user)); 
                // }
                //
            }
            

            return users;
        }

        private List<User> GetAllUsersList()
        {
            var users = ReadUsers();
            
            return JsonConvert.DeserializeObject<List<User>>(users);
        }

    }

}


// private List<User> _users;
// public ReadOnlyCollection<User> Users => _users.AsReadOnly();


//https://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
//AppDomain.CurrentDomain.BaseDirectory, //restarting users over again , don't want to be in bin folder 
///Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/bin/Debug/netcoreapp3.1/DataAccess/Users.json