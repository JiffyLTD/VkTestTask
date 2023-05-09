using UsersAPI.Models;

namespace UsersAPI.BasicAuth.Interfaces
{
    public interface IAuth
    {
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Объект Response</returns>
        public Task<Response> AuthUser(string login, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="user">Объект User</param>
        /// <returns>Ответ проверки пароля пользователя</returns>
        protected bool CheckPassword(string password, User user);

        /// <summary>
        /// Поиск пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Объект Response</returns>
        protected Task<Response> FindUserByLogin(string login);
    }
}
