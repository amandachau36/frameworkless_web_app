using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrameworklessWebApp2.DataAccess
{
    public static class DataManager
    {
        public static List<User> Users { get; private set; } = new List<User>();
        public static void WriteToTextFile(List<User> users)
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

        public static void LoadUsers()
        {
            var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "DataAccess", "Users.json"));

            var json= sr.ReadToEnd();
            
            sr.Close();
            
            Users = JsonConvert.DeserializeObject<List<User>>(json);
        }
        
    }

}

//https://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
//AppDomain.CurrentDomain.BaseDirectory, ;
///Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/bin/Debug/netcoreapp3.1/DataAccess/Users.json