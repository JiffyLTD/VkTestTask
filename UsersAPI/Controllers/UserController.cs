using Microsoft.AspNetCore.Mvc;
using UsersAPI.BasicAuth.Interfaces;
using UsersAPI.Models.ViewModels;
using UsersAPI.Repositories.Interfaces;
using UsersAPI.Validations.Interfaces;

namespace UsersAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _iUser;
        private readonly IUserValidator _userValidator;
        private readonly IAuth _iAuth;

        public UserController(IUser iUser, IUserValidator userValidator, IAuth iAuth)
        {
            _iUser = iUser;
            _userValidator = userValidator;
            _iAuth = iAuth;
        }

        [HttpGet("allUsers")]
        public async Task<IResult> GetUsers(string login, string password)
        {
            //Авторизация пользователя
            var authResult = await _iAuth.AuthUser(login, password);

            if (!authResult.Status)
                return Results.BadRequest(authResult);

            var result = await _iUser.GetUsers();

            return result.Status ? Results.Ok(result) : Results.Problem(result.Message);
        }

        [HttpGet("{page:int}&{limit:int}")]
        public async Task<IResult> GetUsers(int page, int limit, string login, string password)
        {
            //Авторизация пользователя
            var authResult = await _iAuth.AuthUser(login, password);

            if (!authResult.Status)
                return Results.BadRequest(authResult);

            var result = await _iUser.GetUsers(page, limit);

            return result.Status ? Results.Ok(result) : Results.Problem(result.Message);
        }

        [HttpGet("getById")]
        public async Task<IResult> GetUserById(Guid id, string login, string password)
        {
            //Авторизация пользователя
            var authResult = await _iAuth.AuthUser(login, password);

            if (!authResult.Status)
                return Results.BadRequest(authResult);

            var result = await _iUser.GetUserById(id);

            return result.Status ? Results.Ok(result) : Results.Problem(result.Message);
        }

        [HttpPost("")]
        public async Task<IResult> AddUser(RegisterFormViewModel model, string login, string password)
        {
            //Авторизация пользователя
            var authResult = await _iAuth.AuthUser(login, password);

            if (!authResult.Status)
                return Results.BadRequest(authResult);

            // Валидация модели
            var validationResult = await _userValidator.ValidateInput(model);

            if (!validationResult.Status) return Results.BadRequest(validationResult.Message);

            // Добавить пользователя в систему
            var addUserResult = await _iUser.AddUser(model.Login, model.Password, model.IsAdmin);
            if (!addUserResult.Status)
                return Results.Problem(addUserResult.Message);

            return Results.Ok(addUserResult);
        }

        [HttpDelete("")]
        public async Task<IResult> DeleteUser(Guid id, string login, string password)
        {
            //Авторизация пользователя
            var authResult = await _iAuth.AuthUser(login, password);

            if (!authResult.Status)
                return Results.BadRequest(authResult);

            var result = await _iUser.DeleteUser(id);

            return result.Status ? Results.Ok(result) : Results.Problem(result.Message);
        }
    }
}
