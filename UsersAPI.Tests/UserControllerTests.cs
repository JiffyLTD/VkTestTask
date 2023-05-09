using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UsersAPI.Models;
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
        [InlineData("api/v1/users")]
        public async void POST_AddUser_Test(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            var userModels = RegisterFormViewModelData();


            // Act
            var responseUser = await client.PostAsJsonAsync(url, userModels[0]); // добавляем пользователя получаем Success
            var responseUserTwo = await client.PostAsJsonAsync(url, userModels[1]); // добавляем пользователя с таким же логином - получаем BadRequest(Ошибка создания. Повторите попытку позже)
            Thread.Sleep(5000);
            var responseUserThree = await client.PostAsJsonAsync(url, userModels[2]); // добавляем пользователя с таким же логином через 5 секунд - получаем BadRequest(Ошибка создания. Данный логин уже занят)
            var responseAdmin = await client.PostAsJsonAsync(url, userModels[3]); // добавляем первого админа получаем Success
            var responseAdminTwo = await client.PostAsJsonAsync(url, userModels[4]); // добавляем второго админа получаем BadRequest(Ошибка создания. Количество администраторов в системе не более 1)

            var jsonResponseUser = await responseUser.Content.ReadFromJsonAsync<JsonResponse>();
            var user = jsonResponseUser.User;
            var jsonResponseAdmin = await responseAdmin.Content.ReadFromJsonAsync<JsonResponse>();
            var admin = jsonResponseAdmin.User;

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseUser.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseUserTwo.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseUserThree.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAdmin.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseAdminTwo.StatusCode);

            Assert.Equal("User", user.UserGroup.Code);
            Assert.Equal("Admin", admin.UserGroup.Code);
        }

        [Fact]
        public async void DELETE_DeleteUser_Test()
        {
            // Arrange
            var client = _factory.CreateClient();
            RegisterFormViewModel userModel = new()
            {
                Login = "Login",
                Password = "password",
                IsAdmin = false
            };

            // Act
            var responseUser = await client.PostAsJsonAsync("api/v1/users", userModel);// создаем пользователя чтобы потом удалить
            var jsonResponse = await responseUser.Content.ReadFromJsonAsync<JsonResponse>();
            var user = jsonResponse.User;

            var response = await client.DeleteAsync($"api/v1/users?id={user.Id}");
            var jsonResponseAfterDelete = await response.Content.ReadFromJsonAsync<JsonResponse>();
            var userAfterDelete = jsonResponseAfterDelete.User;

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseUser.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Active", user.UserState.Code);
            Assert.Equal("Blocked", userAfterDelete.UserState.Code);
        }

        [Fact]
        public async void GET_GetById_Test()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var responseUser = await client.PostAsJsonAsync("api/v1/users",// создаем пользователя чтобы потом получить
               new RegisterFormViewModel()
               {
                   Login = "Login1",
                   Password = "password2",
                   IsAdmin = false
               });
            var jsonResponse = await responseUser.Content.ReadFromJsonAsync<JsonResponse>();
            var user = jsonResponse.User;
            var response = await client.GetAsync($"api/v1/users/getById?id={user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Login1", user.Login);
            Assert.Equal("Active", user.UserState.Code);
            Assert.Equal("User", user.UserGroup.Code);
        }

        [Theory]
        [InlineData("api/v1/users/allUsers")]
        [InlineData("api/v1/users/1&1")]
        public async void GET_GetAll_Test(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonResponse>();
            var users = jsonResponse.Users;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, users.Count);
        }

        private List<RegisterFormViewModel> RegisterFormViewModelData()
        {
            return new List<RegisterFormViewModel>
            {
                new RegisterFormViewModel
                {
                    Login = "user",
                    Password = "user",
                    IsAdmin = false
                },
                new RegisterFormViewModel
                {
                    Login = "user",
                    Password = "userTwo",
                    IsAdmin = false
                },
                new RegisterFormViewModel
                {
                    Login = "user",
                    Password = "userThree",
                    IsAdmin = false
                },
                new RegisterFormViewModel
                {
                    Login = "admin",
                    Password = "admin",
                    IsAdmin = true
                },
                new RegisterFormViewModel
                {
                    Login = "adminTwo",
                    Password = "adminTwo",
                    IsAdmin = true
                }
            };
        }
        public class JsonResponse
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public User User { get; set; }
            public List<User> Users { get; set; }
        }
    }
}