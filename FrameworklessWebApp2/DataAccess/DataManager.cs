using System.Collections.Generic;
using System.Linq;
using System.Net;


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
            user.SetIsDeleted(false);

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
                throw new ObjectNotFoundException($"User not found: id{id}");

            return users[index];

        }

        public User UpdateUser(int id, User user)
        {
            var users = _database.Read();

            var index = users.FindIndex(x => x.Id == id && x.IsDeleted == false);

            if (index < 0) 
                throw new ObjectNotFoundException($"User not found: id{id}");

            var propertiesToUpdate = user.GetType().GetProperties();

            foreach (var prop in propertiesToUpdate)
            {
                var value = prop.GetValue(user);

                if (value is null || prop.Name == "Id" || prop.Name == "IsDeleted") continue;  

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
                throw new ObjectNotFoundException($"User not found: id{id}");

            users[index].SetIsDeleted(true);
            
            _database.Write(users);
            
        }
        
    }

  
}


// private List<User> _users;
// public ReadOnlyCollection<User> Users => _users.AsReadOnly();


//https://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
//AppDomain.CurrentDomain.BaseDirectory, //restarting users over again , don't want to be in bin folder 
///Users/amanda.chau/fma/FrameworklessWebApp2/FrameworklessWebApp2/bin/Debug/netcoreapp3.1/DataAccess/Users.json