using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Controllers;
using UsersAPI.Models;
using UsersAPI.Models.ViewModels;
using UsersAPI.Repositories.Interfaces;
using UsersAPI.Validations.Interfaces;

namespace UsersAPI.Tests
{
    public class UserControllerTests 
    {
        HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7133") };
        [Fact]
        public async void AddNewUserTest()
        {
            var response = await httpClient.GetAsync("allUsers");

            
        }

        [Fact]
        public void GetUsersTest()
        {
           
        }

        private async static Task<Response> GetTestUsers()
        {
            var users = new List<User>
            {
                new User ("admin", "admin", true),
                new User ("aboba", "aboba", false),
                new User ("biba", "biba", false),
                new User ("gosha", "gosha", false),
                new User ("jiffy", "jiffy", false)
            };

            return new Response("Список пользователей", true, users);
        }
    }
}
