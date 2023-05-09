namespace UsersAPI.Models
{
    public class Response
    {
        /// <summary>
        /// Конструктор создания объекта Response
        /// </summary>
        /// <param name="message">Сообщение ответа</param>
        /// <param name="status">Статус ответа об операции</param>
        public Response(string message, bool status)
        {
            Message = message;
            Status = status;
        }

        /// <summary>
        /// Конструктор создания объекта Response
        /// </summary>
        /// <param name="message">Сообщение ответа</param>
        /// <param name="status">Статус ответа об операции</param>
        /// <param name="user">Объект User</param>
        public Response(string message, bool status, User user) 
        {
            Message = message;
            Status = status;
            User = user;
        }

        /// <summary>
        /// Конструктор создания объекта Response
        /// </summary>
        /// <param name="message">Сообщение ответа</param>
        /// <param name="status">Статус ответа об операции</param>
        /// <param name="users">Список объектов Users</param>
        public Response(string message, bool status, List<User> users) 
        {
            Message = message;
            Status = status;
            Users = users;
        }

        public string Message { get; set; } = null!;
        public bool Status { get; set; }
        public User? User { get; set; }
        public List<User>? Users { get; set; }
    }
}
