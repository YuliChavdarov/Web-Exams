using Git.Data;
using Git.Data.Models;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Git.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext context;

        public UsersService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public string CreateUser(string username, string email, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Email = email,
                Password = ComputeHash(password),
                Role = IdentityRole.User
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user.Id;

        }

        public string GetUserId(string username, string password)
        {
            return context.Users.FirstOrDefault(x => x.Username == username && x.Password == ComputeHash(password))?.Id;
        }

        public bool IsEmailAvailable(string email)
        {
            return !context.Users.Any(x => x.Email == email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return !context.Users.Any(x => x.Username == username);
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
