using System.Collections.Generic;

namespace FrameworklessWebApp2.DataAccess
{
    public interface IDatabase
    {
        void Write(List<User> users);

        List<User> Read();
    }
}