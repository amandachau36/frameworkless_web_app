using System.Collections.Generic;
using System.Net;

namespace FrameworklessWebApp2.Controllers

{
    public interface IController<T> where T : class
    {
        T Post(T model);
        List<T> Get();
        T Get(int id);
        T Put(T model, int id);
        void Delete(int id);

    }
}