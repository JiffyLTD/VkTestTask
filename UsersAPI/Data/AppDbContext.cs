namespace UsersAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using UsersAPI.Models;

    public class AppDbContext : DbContext
    {
        // Создание БД если она не создана, при вызове конструктора
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();

            // Добавляем админа от лица которого будем производить запросы(если бд пустая и нет других пользователей)
            User user = new("admin", "admin", true);

            if(Users.ToList().Count == 0)
                Users.Add(user);

            SaveChanges();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }
    }
}
