using UsersAPI.Models;

namespace UsersAPI.Repositories.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// Получение последнего добавленного(по времени создания) пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Объект Response</returns>
        public Task<Response> GetLastUserByLogin(string login);
        /// <summary>
        /// Получение пользователя по его ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Объект Response</returns>
        public Task<Response> GetUserById(Guid id);

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
        /// Создание нового пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="isAdmin">Будет ли пользователь админом</param>
        /// <returns>Объект Response</returns>
        public Task<Response> AddUser(string login, string password, bool isAdmin);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="user">Объект пользователь</param>
        /// <returns>Объект Response</returns>
        public Task<Response> DeleteUser(User user); 
    }
}
