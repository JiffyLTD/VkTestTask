using System.ComponentModel.DataAnnotations.Schema;

namespace UsersAPI.Models
{
    public class User
    {
        public User()
        {
        }
        /// <summary>
        /// Конструтор для создания пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="isAdmin">Будет ли пользователь админом</param>
        public User(string login, string password, bool isAdmin)
        {
            Id = Guid.NewGuid();
            Login = login;
            Password = password;
            CreatedDate = DateTime.Now;

            UserGroup = new UserGroup(isAdmin);
            UserGroupId = UserGroup.Id;

            UserState = new UserState();
            UserStateId = UserState.Id;
        }
        /// <summary>
        /// Конструтор для создания пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="isAdmin">Будет ли пользователь админом</param>
        /// <param name="userGroupDesc">Описание группы пользователя</param>
        /// <param name="userStateDesc">Описание состояния пользователя</param>
        public User(string login, string password, bool isAdmin, string userGroupDesc, string userStateDesc)
        {
            Id = Guid.NewGuid();
            Login = login;
            Password = password;
            CreatedDate = DateTime.Now;

            UserGroup = new UserGroup(isAdmin, userGroupDesc);
            UserGroupId = UserGroup.Id;

            UserState = new UserState(userStateDesc);
            UserStateId = UserState.Id;
        }

        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        [ForeignKey("UserGroup")]
        public Guid UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        [ForeignKey("UserState")]
        public Guid UserStateId { get; set; }
        public UserState UserState { get; set; }
    }
}
