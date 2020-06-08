using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrameworklessWebApp2.DataAccess
{
    public class TextfileDatabase : IDatabase
    {
        private readonly string _path;

        public TextfileDatabase(string path)
        {
            _path = path;
        }
        public void Write(List<User> users)
        {
            var usersList = new JArray(
                from u in users
                select new JObject(
                    new JProperty("username", u.Username),
                    new JProperty("name", u.Name),
                    new JProperty("location", u.Location),
                    new JProperty("id", u.Id),
                    new JProperty("isDeleted", u.IsDeleted)
                )
            );

            var sw = new StreamWriter(_path);
            
            sw.WriteLine(usersList);
            
            sw.Flush();

            sw.Close();
        }

        public List<User> Read()
        {
            var sr = new StreamReader(_path);

            var json= sr.ReadToEnd();
            
            sr.Close();

            return JsonConvert.DeserializeObject<List<User>>(json);
        }
        

       
    }
}