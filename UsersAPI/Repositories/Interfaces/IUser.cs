using UsersAPI.Models;

namespace UsersAPI.Repositories.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// Получение пользователя по его логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Объект Response</returns>
        public Task<Response> GetUserByLogin(string login);

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns>Объект Response</returns>
        public Task<Response> GetUsers();

        /// <summary>
        /// Получение всех пользователей (пагинация)
        /// </summary>
        /// <param name="page">Страница которую необходимо получить</param>
        /// <param name="limit">Количество пользователей на странице</param>
        /// <returns>Объект Response</returns>
        public Task<Response> GetUsers(int page, int limit);

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="user">Объект пользователь</param>
        /// <returns>Объект Response</returns>
        public Task<Response> AddUser(User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="user">Объект пользователь</param>
        /// <returns>Объект Response</returns>
        public Task<Response> DeleteUser(User user); 
    }
}
