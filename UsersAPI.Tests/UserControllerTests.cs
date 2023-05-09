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
        [InlineData("api/v1/users?login=admin&password=admin")]
        public async void POST_AddUser_Test(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            var userModels = RegisterFormViewModelData();


            // Act
            var responseUser = await client.PostAsJsonAsync(url, userModels[0]); // ��������� ������������ �������� Success
            var responseUserTwo = await client.PostAsJsonAsync(url, userModels[1]); // ��������� ������������ � ����� �� ������� -
                                                                                    // �������� BadRequest(������ ��������. ��������� ������� �����)
            Thread.Sleep(5000);
            var responseUserThree = await client.PostAsJsonAsync(url, userModels[2]); // ��������� ������������ � ����� �� ������� ����� 5 ������ -
                                                                                      // �������� BadRequest(������ ��������. ������ ����� ��� �����)
            var responseAdmin = await client.PostAsJsonAsync(url, userModels[3]); // ��������� ������� ������ �������� BadRequest(������ ��������.
                                                                                     // ���������� ��������������� � ������� �� ����� 1)

            var jsonResponseUser = await responseUser.Content.ReadFromJsonAsync<JsonResponse>();
            var user = jsonResponseUser.User;

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseUser.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseUserTwo.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseUserThree.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseAdmin.StatusCode);

            Assert.Equal("User", user.UserGroup.Code);
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
            var responseUser = await client.PostAsJsonAsync("api/v1/users?login=admin&password=admin", userModel);// ������� ������������ ����� ����� �������
            var jsonResponse = await responseUser.Content.ReadFromJsonAsync<JsonResponse>();
            var user = jsonResponse.User;

            var response = await client.DeleteAsync($"api/v1/users?id={user.Id}&login=admin&password=admin");
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
            var responseUser = await client.PostAsJsonAsync("api/v1/users?login=admin&password=admin",// ������� ������������ ����� ����� ��������
               new RegisterFormViewModel()
               {
                   Login = "Login1",
                   Password = "password2",
                   IsAdmin = false
               });
            var jsonResponse = await responseUser.Content.ReadFromJsonAsync<JsonResponse>();
            var user = jsonResponse.User;
            var response = await client.GetAsync($"api/v1/users/getById?id={user.Id}&login=admin&password=admin");
            var badResponse = await client.GetAsync($"api/v1/users/getById?id={user.Id}&12345&12345");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, badResponse.StatusCode);
            Assert.Equal("Login1", user.Login);
            Assert.Equal("Active", user.UserState.Code);
            Assert.Equal("User", user.UserGroup.Code);
        }

        [Theory]
        [InlineData("api/v1/users/allUsers?login=admin&password=admin")]
        [InlineData("api/v1/users/1&1?login=admin&password=admin")]
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