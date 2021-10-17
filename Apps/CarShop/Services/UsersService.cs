using CarShop.Data;
using CarShop.Data.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CarShop.Services
{
    public class UsersService: IUsersService
    {
        private readonly ApplicationDbContext context;

        public UsersService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string Create(string username, string email, string password, bool isMechanic)
        {
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Email = email,
                Password = ComputeHash(password),
                IsMechanic = isMechanic
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user.Id;
        }

        public string GetUserId(string username, string password)
        {
            return this.context.Users
                .FirstOrDefault(x => x.Username == username && x.Password == ComputeHash(password))
                ?.Id;
        }

        public bool isClient(string userId)
        {
            return context.Users.First(x => x.Id == userId).IsMechanic == false;
        }

        public bool isMechanic(string userId)
        {
            return context.Users.First(x => x.Id == userId).IsMechanic;
        }

        public bool IsUsernameAvailable(string username)
        {
            return this.context.Users.All(x => x.Username != username);
        }

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
}
