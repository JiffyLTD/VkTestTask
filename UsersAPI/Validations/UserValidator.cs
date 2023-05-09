using UsersAPI.Models;
using UsersAPI.Models.ViewModels;
using UsersAPI.Repositories.Interfaces;
using UsersAPI.Validations.Interfaces;

namespace UsersAPI.Validations
{
    public class UserValidator : IUserValidator
    {
        private readonly IUser _iUser;

        public UserValidator(IUser iUser)
        {
            _iUser = iUser;
        }

        public async Task<bool> AdditionIsComplete(string login)
        {
            var result = await _iUser.GetLastUserByLogin(login);

            if (result.Status && result.User != null)
                    return AdditionTimeIsOver(result.User.CreatedDate);

            return true;
        }

        private static bool AdditionTimeIsOver(DateTime userCreatedDate)
        {
            return userCreatedDate < DateTime.Now.AddSeconds(-5);
        }

        public async Task<bool> AdminPlaceIsEmpty()
        {
            return await _iUser.NumOfAdmins() < 1;
        }

        public async Task<bool> LoginIsUniq(string login)
        {
            var result = await _iUser.GetLastUserByLogin(login);

            return !result.Status;
        }

        public async Task<Response> ValidateInput(RegisterFormViewModel model)
        {
            if (!await AdditionIsComplete(model.Login))
                return new Response("Ошибка создания. Повторите попытку позже", false);

            // Проверить уникальность логина
            if (!await LoginIsUniq(model.Login))
                return new Response("Ошибка создания. Данный логин уже занят", false);

            // Проверить количество администраторов в системе
            if (model.IsAdmin && !await AdminPlaceIsEmpty())
                return new Response("Ошибка создания. Количество администраторов в системе не более 1", false);

            // Вернуть успешный результат
            return new Response("", true);
        }
    }
}
