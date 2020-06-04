using System.Collections.Generic;
using System.Net;
using FrameworklessWebApp2.Models;

namespace FrameworklessWebApp2.Controllers

{
    public interface IController<T> where T : class
    {
        T Post(IModel model);
        List<T> Get();
        T Get(int id);
        T Put(IModel model, int id);
        void Delete(int id);

    }
}