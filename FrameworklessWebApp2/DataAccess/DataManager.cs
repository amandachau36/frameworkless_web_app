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

        private List<User> _users; //TODO: does this mean it has state? 

        public ReadOnlyCollection<User> Users => _users.AsReadOnly();

        public DataManager()
        {
            _users = new List<User>();
        }
        public void WriteToTextFile(ReadOnlyCollection<User> users)
        {
            var usersList = new JArray(
                from u in users
                select new JObject(
                    new JProperty("username", u.Username),
                    new JProperty("name", u.Name),
                    new JProperty("location", u.Location)
                )
            );

            var sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "DataAccess", "Users.json"));
            
            sw.WriteLine(usersList);
            
            sw.Flush();

            sw.Close();

        }

        public void LoadUsers()
        {
            var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "DataAccess", "Users.json"));

            var json= sr.ReadToEnd();
            
            sr.Close();
            
            _users = JsonConvert.DeserializeObject<List<User>>(json);
            
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }
        
    }

}

//TODO: is this right
//https://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
//AppDomain.CurrentDomain.BaseDirectory, ;
///Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/bin/Debug/netcoreapp3.1/DataAccess/Users.json