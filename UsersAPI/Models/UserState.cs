namespace UsersAPI.Models
{
    public class UserState
    {
        /// <summary>
        /// Конструктор для создания состояния пользователя
        /// </summary>
        public UserState()
        {
            Id = Guid.NewGuid();
            Code = "Active";
        }
        /// <summary>
        /// Конструктор для создания состояния пользователя 
        /// </summary>
        /// <param name="description">Описание состояния пользователя</param>
        public UserState(string description)
        {
            Id = Guid.NewGuid();
            Code = "Active";
            Description = description;
        }

        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
    }
}
