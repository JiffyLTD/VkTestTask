namespace UsersAPI.Models.ViewModels
{
    public class RegisterFormViewModel
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; }
    }
}
