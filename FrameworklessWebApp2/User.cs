using System.Reflection;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public class User //model
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        
        [JsonProperty(PropertyName ="name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        public static void SetId(User user, int id)
        {
            user.Id = id;
        }

        
    }
}