namespace UsersAPI.Validations.Interfaces
{
    public interface IUserValidator
    {
        /// <summary>
        /// Получение ответа завершен ли процесс добавления пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Ответ завершен ли процесс добавления пользователя</returns>
        public Task<bool> AdditionIsComplete(string login);
    }
}
