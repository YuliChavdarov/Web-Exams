namespace CarShop.Services
{
    public interface IUsersService
    {
        string GetUserId(string username, string password);

        string Create(string username, string email, string password, bool isMechanic);

        bool IsUsernameAvailable(string username);

        public bool isMechanic(string userId);
        public bool isClient(string userId);
    }
}
