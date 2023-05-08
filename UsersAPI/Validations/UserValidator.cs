using UsersAPI.Repositories.Interfaces;
using UsersAPI.Validations.Interfaces;

namespace UsersAPI.Validations
{
    public class UserValidator : IUserValidator
    {
        private readonly IUser _iUser;

        public UserValidator(IUser iUser)
        {
            _iUser = iUser;
        }

        public async Task<bool> AdditionIsComplete(string login)
        {
            var result = await _iUser.GetLastUserByLogin(login);

            if (result.Status && result.User != null)
            {
                if (AdditionTimeIsOver(result.User.CreatedDate))
                    return true;
                else
                    return false;
            }

            return true;
        }

        private static bool AdditionTimeIsOver(DateTime userCreatedDate)
        {
            return userCreatedDate.Second - DateTime.Now.Second > 5;
        }
    }
}
