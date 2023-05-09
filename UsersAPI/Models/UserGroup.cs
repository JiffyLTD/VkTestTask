namespace UsersAPI.Models
{
    public class UserGroup
    {
        // Конструктор без параметров для EntityFramework
        public UserGroup()
        {
        }

        /// <summary>
        /// Конструктор для создание группы пользователя
        /// </summary>
        /// <param name="isAdmin">Будет ли пользователь админом</param>
        public UserGroup(bool isAdmin)
        {
            Id = Guid.NewGuid();
            if (isAdmin)
                Code = "Admin";
            else
                Code = "User";
        }

        /// <summary>
        /// Конструктор для создание группы пользователя
        /// </summary>
        /// <param name="isAdmin">Будет ли пользователь админом</param>
        /// <param name="description">Описание группы пользователя</param>
        public UserGroup(bool isAdmin, string description)
        {
            Id = Guid.NewGuid();
            if (isAdmin)
                Code = "Admin";
            else
                Code = "User";
            Description = description;
        }

        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
    }
}
