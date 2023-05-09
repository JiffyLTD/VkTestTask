using UsersAPI.Models.ViewModels;
using UsersAPI.Models;

namespace UsersAPI.Validations.Interfaces
{
    public interface IUserValidator
    {
        /// <summary>
        /// Общий метод всех проверок
        /// </summary>
        /// <param name="model">Модель регистрации пользователя</param>
        /// <returns>Объект Response</returns>
        public Task<Response> ValidateInput(RegisterFormViewModel model);

        /// <summary>
        /// Проверка завершен ли процесс добавления пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Завершен ли процесс добавления пользователя</returns>
        protected Task<bool> AdditionIsComplete(string login);

        /// <summary>
        /// Проверка на количество добавленных админов
        /// </summary>
        /// <returns>Админов в системе более 1 или нет</returns>
        protected Task<bool> AdminPlaceIsEmpty();

        /// <summary>
        /// Проверка логина на уникальность
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Уникальный ли логин в системе</returns>
        protected Task<bool> LoginIsUniq(string login);
    }
}
