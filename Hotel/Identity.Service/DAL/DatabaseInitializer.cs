using Identity.Service.Services;
using Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Service.DAL
{
    public static class DatabaseInitializer
    {
        public static void Seed(UserService userService)
        {
            if (!userService.GetUsers().Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        Login = "admin",
                        Password = "root",
                        Role = "Admin"
                    },
                    new User()
                    {
                        Login = "worker",
                        Password = "root",
                        Role = "Employee"
                    },
                };

                users.ForEach(user => userService.Create(user));
            }   
        }
    }
}
