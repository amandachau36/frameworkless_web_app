using System;
using System.Runtime.CompilerServices;
using FrameworklessWebApp2.DataAccess;

namespace FrameworklessWebApp2.Resources
{
    public class ResourceFactory
    {
        public static IResource CreateResource((Resource, int?) resource, DataManager dataManager) =>
            
            resource.Item1 switch 
            {
                Resource.Users => new UsersResource(dataManager),
                Resource.User => new UserResource(dataManager, resource.Item2.GetValueOrDefault()),
                _ => throw new ArgumentException("Enum does not exist") 
            
            };
    }
    
   
}