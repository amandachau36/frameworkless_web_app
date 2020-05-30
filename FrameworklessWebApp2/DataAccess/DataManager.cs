using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        public List<User> AddUser(User user)
        {
            var users = GetAllUsersList();
            
            var id = users.Last().Id + 1;                            //TODO: not ideal
            User.SetId(user, id);
            
            users.Add(user);
            
            return users;
        }
        public string GetUsers() 
        {
            var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "DataAccess", "Users.json"));

            var json= sr.ReadToEnd();
            
            sr.Close();

            return json;
          
        }

        public string GetUser(int? id)
        {
            var users = GetAllUsersList();

            var user = users.Find(x => x.Id == id);

            return JsonConvert.SerializeObject(user);

        }

        private List<User> GetAllUsersList()
        {
            var users = GetUsers();
            
            return JsonConvert.DeserializeObject<List<User>>(users);
        }

    }

}


// private List<User> _users;
// public ReadOnlyCollection<User> Users => _users.AsReadOnly();


//https://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
//AppDomain.CurrentDomain.BaseDirectory, //restarting users over again , don't want to be in bin folder 
///Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/bin/Debug/netcoreapp3.1/DataAccess/Users.json