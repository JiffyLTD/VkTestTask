using UsersAPI.BasicAuth.Interfaces;
using UsersAPI.Models;
using UsersAPI.Repositories.Interfaces;

namespace UsersAPI.BasicAuth
{
    public class UserAuth : IAuth
    {
        private readonly IUser _iUser;

        public UserAuth(IUser iUser)
        {
            _iUser = iUser;
        }

        public async Task<Response> AuthUser(string login, string password)
        {
            var findUserResult = await FindUserByLogin(login);

            // Проверка на наличие пользователя в БД
            if (!findUserResult.Status)
                return new Response("Неверный логин или пароль", false);

            // Проверка пароля пользователя
            if(!CheckPassword(password, findUserResult.User))
                return new Response("Неверный логин или пароль", false);

            return new Response("Пользователь авторизован",true);
        }

        public bool CheckPassword(string password, User user)
        {
            return password == user.Password;
        }

        public async Task<Response> FindUserByLogin(string login)
        {
            var result = await _iUser.GetUserByLogin(login);

            return result;
        }
    }
}
