using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using UsersAPI.Models.ViewModels;

namespace UsersAPI.Tests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("api/v1/users/allUsers")]
        [InlineData("api/v1/users/1&1")]
        public async void GET_GetAll_ReturnSuccessStatusCode(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/v1/users/getById?id=1")] // из за несуществующего ID получаем BadRequest
        public async void GET_GetById_ReturnBadRequestStatusCode(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/v1/users?id=1")] // из за несуществующего ID получаем BadRequest
        public async void DELETE_DeleteUser_ReturnBadRequestStatusCode(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/v1/users")]
        public async void POST_AddUser_ReturnStatusCode(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            RegisterFormViewModel userModel = new() 
            {
                Login = "user",
                Password = "user",
                IsAdmin = false 
            }; 
            RegisterFormViewModel userModelTwo = new() 
            { 
                Login = "user",
                Password = "userTwo", 
                IsAdmin = false 
            }; 
            RegisterFormViewModel userModelThree = new() 
            { 
                Login = "user",
                Password = "userThree",
                IsAdmin = false 
            }; 
            RegisterFormViewModel adminModel = new()
            {
                Login = "admin",
                Password = "admin",
                IsAdmin = true 
            };
            RegisterFormViewModel adminModelTwo = new()
            { 
                Login = "adminTwo", 
                Password = "adminTwo", 
                IsAdmin = true
            };

            // Act
            var responseUser = await client.PostAsJsonAsync(url, userModel); // добавляем пользователя получаем Success
            var responseUserTwo = await client.PostAsJsonAsync(url, userModelTwo); // добавляем пользователя с таким же логином - получаем BadRequest(Ошибка создания. Повторите попытку позже)
            Thread.Sleep(5000);
            var responseUserThree = await client.PostAsJsonAsync(url, userModelThree); // добавляем пользователя с таким же логином через 5 секунд - получаем BadRequest(Ошибка создания. Данный логин уже занят)
           
            var responseAdmin = await client.PostAsJsonAsync(url, adminModel); // добавляем первого админа получаем Success
            var responseAdminTwo = await client.PostAsJsonAsync(url, adminModelTwo); // добавляем второго админа получаем BadRequest(Ошибка создания. Количество администраторов в системе не более 1)

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseUser.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseUserTwo.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseUserThree.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAdmin.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseAdminTwo.StatusCode);
        }
    }
}