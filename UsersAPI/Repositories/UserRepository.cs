using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
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

        public async Task<Response> AddUser(string login, string password, bool isAdmin)
        {
            try
            {
                var user = new User(login, password, isAdmin);

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return new Response("Пользователь успешно добавлен", true, user);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> DeleteUser(Guid id)
        {
            try
            {
                var user = await _db.Users.Include(x => x.UserState).FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    user.UserState.Code = "Blocked";
                    await _db.SaveChangesAsync();

                    return new Response("Пользователь успешно удален", true, user);
                }

                return new Response("Пользователь не найден", false);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> GetLastUserByLogin(string login)
        {
            try
            {
                var user = await _db.Users.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(x => x.Login == login);

                if (user != null)
                    return new Response("Пользователь найден", true, user);
                else
                    return new Response("Пользователь с таким логином не найден", false);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, false);
            }
        }

        public async Task<Response> GetUserById(Guid id)
        {
            try
            {
                var user = await _db.Users.Include(x => x.UserGroup)
                    .Include(x => x.UserState).FirstOrDefaultAsync(u => u.Id == id);

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

        public async Task<Response> GetUserByLogin(string login)
        {
            try
            {
                var user = await _db.Users.Include(x => x.UserGroup)
                    .Include(x => x.UserState).FirstOrDefaultAsync(u => u.Login == login);

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
                var users = await _db.Users.Include(x => x.UserGroup)
                    .Include(x => x.UserState).ToListAsync();

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
                var allUsers = await _db.Users.Include(x => x.UserGroup)
                    .Include(x => x.UserState).ToListAsync();

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

        public async Task<int> NumOfAdmins()
        {
            var allAdmins = await _db.Users.Include(x => x.UserGroup).
                Where(x => x.UserGroup.Code == "Admin").ToListAsync();

            return allAdmins.Count;
        }
    }
}
