using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmIT_Api.Models;
using FarmIT_Api.Helpers;

namespace FarmIT_Api.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Username = "xYvN7EOnhzdnTinyuq8amuhzRaBxNeOeeJyrp3/L0+Y=", Password = "f4eqUIMzUI4UyBwnFJOPhji8D2umvEC2GzJFDOmRzB8=" }  // demo
        };
    //new User { Username = "gO9SJ8+gLFsxgvzQMpWOGiu+3xuYppm+6zVxUuS/jEE=", Password = "XnQTW1+Hq4oF8Ovgy7ZWb5YPTE8q6QXN4Gd1I3qqoAg=" }stag
    //new User { Username = "93QlJLRF4CSo4tQVEaX4TdoqNFnc0c0SVwXlEDdP010=", Password = "S7OkYJ89dZvCJMqK78v8VUI/KZ/rvEgOKv37yy4L5eY=" }live
    //new User { Username = "mcmtd23qdpilpMg6yJewo/h5W8gur2WDWBlm7ArOHw0=", Password = "fTHXl6DI3xNuWA0ykAD2zl+ulkzViENN9+kAtSn7L4E=" } test

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        //public async Task<IEnumerable<User>> GetAll()
        //{
        //    return await Task.Run(() => _users.WithoutPasswords());
        //}
    }
}
