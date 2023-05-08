using Microsoft.AspNetCore.Mvc;
using UsersAPI.Models;
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

        public UserController(IUser iUser, IUserValidator userValidator)
        {
            _iUser = iUser;
            _userValidator = userValidator;
        }

        [HttpGet("")]
        public async Task<IResult> GetUsers()
        {
            var result = await _iUser.GetUsers();

            if (result.Status)
                return Results.Ok(result);
            else
                return Results.Problem(result.Message);
        }

        [HttpGet("{page:int}&{limit:int}")]
        public async Task<IResult> GetUsers(int page, int limit)
        {
            var result = await _iUser.GetUsers(page, limit);

            if (result.Status)
                return Results.Ok(result);
            else
                return Results.Problem(result.Message);
        }

        [HttpGet("getById")]
        public async Task<IResult> GetUserById(Guid id)
        {
            var result = await _iUser.GetUserById(id);

            if(result.Status)
                return Results.Ok(result);
            else
                return Results.NotFound(result.Message);
        }

        [HttpPost("")] // не хватает опыта чтобы красиво расписать метод:(
        public async Task<IResult> AddUser(RegisterFormViewModel model)
        {
            if (await _userValidator.AdditionIsComplete(model.Login)) // проверка завершения создания
            {
                if (await _userValidator.LoginIsUniq(model.Login)) // проверка уникальности логина
                {
                    if (model.IsAdmin) // если пользователь указал что будет админом
                    {
                        if (await _userValidator.AdminPlaceIsEmpty()) // проверка есть ли админы в системе
                        {
                            var result = await _iUser.AddUser(model.Login, model.Password, model.IsAdmin);

                            if (result.Status)
                                return Results.Ok(result);
                            else
                                return Results.Problem(result.Message);
                        }

                        return Results.BadRequest("Ошибка создания. Количество администраторов в системе не более 1");
                    }
                    else // если пользователь указал что не будет админом
                    {
                        var result = await _iUser.AddUser(model.Login, model.Password, model.IsAdmin);

                        if (result.Status)  
                            return Results.Ok(result);
                        else
                            return Results.Problem(result.Message);
                    }
                }

                return Results.BadRequest("Ошибка создания. Данный логин уже занят");
            }

            return Results.BadRequest("Ошибка создания. Повторите попытку позже");
        }

        [HttpDelete("")]
        public async Task<IResult> DeleteUser(Guid id)
        {
            var result = await _iUser.DeleteUser(id);

            if(result.Status)
                return Results.Ok(result);
            else
                return Results.Problem(result.Message);
        }
    }
}
