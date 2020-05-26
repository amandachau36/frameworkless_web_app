using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FrameworklessWebApp2.DataAccess
{
    public static class DataManager
    {
        public static void WriteToTextFile()
        {
            var users = new JArray(
                from u in Request.Users
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
    }

}