using System.Reflection;
using FrameworklessWebApp2.Controllers;
using FrameworklessWebApp2.Models;
using Newtonsoft.Json;

namespace FrameworklessWebApp2
{
    public class User : IModel
    {
       
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        
        [JsonProperty(PropertyName ="name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        [JsonProperty(PropertyName = "isDeleted")]
        public bool IsDeleted { get; private set; }
        
        public void SetId(int id)
        {
            Id = id;
        }
        
        public void SetIsDeletedToTrue()
        {
            IsDeleted = true;
        }

        
    }
}