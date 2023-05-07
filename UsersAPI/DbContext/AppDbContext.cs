﻿namespace UsersAPI.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using UsersAPI.Models;

    public class AppDbContext : DbContext
    {
        // Создание БД если она не создана, при вызове конструктора
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

        // Настройка моделей БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne<UserGroup>();
            modelBuilder.Entity<User>().HasOne<UserState>();

            base.OnModelCreating(modelBuilder);
        }
    }
}