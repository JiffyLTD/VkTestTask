namespace UsersAPI.Validations.Interfaces
{
    public interface IUserValidator
    {
        /// <summary>
        /// Проверка завершен ли процесс добавления пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Завершен ли процесс добавления пользователя</returns>
        public Task<bool> AdditionIsComplete(string login);

        /// <summary>
        /// Проверка на количество добавленных админов
        /// </summary>
        /// <returns>Админов в системе более 1 или нет</returns>
        public Task<bool> AdminPlaceIsEmpty();
    }
}
