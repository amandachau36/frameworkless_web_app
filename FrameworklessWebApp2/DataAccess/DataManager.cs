using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrameworklessWebApp2.DataAccess
{
    public static class DataManager
    {
        public static List<User> Users { get; private set; } = new List<User>();
        public static void WriteToTextFile()
        {
            var users = new JArray(
                from u in Users
                select new JObject(
                    new JProperty("username", u.Username),
                    new JProperty("name", u.Name),
                    new JProperty("location", u.Location)
                )
            );

            var sw = new StreamWriter(
                $"/Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/DataAccess/Users.json");
                //TODO: why doesn't this workPath.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataAccess", "Users.txt"));
                
            sw.WriteLine(users);

            sw.Close();

        }

        public static void LoadUsers()
        {
            var sr = new StreamReader($"/Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/DataAccess/Users.json");

            var json= sr.ReadToEnd();
            
            Users = JsonConvert.DeserializeObject<List<User>>(json);
        }
        
    }

}