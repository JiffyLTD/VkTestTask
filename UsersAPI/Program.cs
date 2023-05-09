using Microsoft.EntityFrameworkCore;
using UsersAPI.BasicAuth;
using UsersAPI.BasicAuth.Interfaces;
using UsersAPI.Data;
using UsersAPI.Repositories;
using UsersAPI.Repositories.Interfaces;
using UsersAPI.Validations;
using UsersAPI.Validations.Interfaces;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Подключение БД
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Регистрируем сервисы
        builder.Services.AddScoped<IUser, UserRepository>();
        builder.Services.AddScoped<IUserValidator, UserValidator>();
        builder.Services.AddScoped<IAuth, UserAuth>();

        var app = builder.Build();

        // Решаем проблему с типом Datetime в Postgre
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}