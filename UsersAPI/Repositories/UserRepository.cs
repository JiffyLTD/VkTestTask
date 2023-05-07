using Microsoft.EntityFrameworkCore;
using UsersAPI.DbContext;
using UsersAPI.Models;
using UsersAPI.Repositories.Interfaces;

namespace UsersAPI.Repositories
{
    public class UserRepository : IUser
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Response> AddUser(User user)
        {
            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return new Response("Пользователь успешно добавлен", true);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> DeleteUser(User user)
        {
            try
            {
                user.UserState.Code = "Blocked";
                await _db.SaveChangesAsync();

                return new Response("Пользователь успешно удален", true);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> GetUserByLogin(string login)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login);

                if (user != null) 
                    return new Response("Пользователь успешно найден", true, user);
                else
                    return new Response("Пользователь не найден", false);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> GetUsers()
        {
            try
            {
                var users = await _db.Users.ToListAsync();

                return new Response("Список пользователей", true, users);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> GetUsers(int page, int limit)
        {
            try
            {
                var allUsers = await _db.Users.ToListAsync();

                var users = new List<User>();

                for (int i = 0; i < limit; i++)
                {
                    try
                    {
                        users.Add(allUsers[i + limit * page - limit]);
                    }
                    catch{}
                }

                return new Response($"Список пользователей: страница - {page}, количество - {limit}", true, users);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }
    }
}
