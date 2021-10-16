using Git.Services;
using Git.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if(this.IsUserSignedIn())
            {
                return this.Redirect("/Repositories/All");
            }

            if (input.Username == null || input.Username.Length < GlobalConstants.UsernameMinLength
                || input.Username.Length > GlobalConstants.UsernameMaxLength)
            {
                return this.Error(GlobalConstants.UsernameLengthError);
            }

            if (string.IsNullOrWhiteSpace(input.Email) || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error(GlobalConstants.EmailError);
            }

            if (input.Password == null || input.Password.Length < GlobalConstants.PasswordMinLength
                || input.Password.Length > GlobalConstants.PasswordMaxLength)
            {
                return this.Error(GlobalConstants.PasswordLengthError);
            }

            if(input.ConfirmPassword != input.Password)
            {
                return this.Error(GlobalConstants.ConfirmPasswordError);
            }

            if (!usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error(GlobalConstants.UsernameTakenError);
            }

            if (!usersService.IsEmailAvailable(input.Email))
            {
                return this.Error(GlobalConstants.EmailTakenError);
            }

            usersService.CreateUser(input.Username, input.Email, input.Password);

            return this.Redirect("Login");
        }

        [HttpGet]
        public HttpResponse Login()
        {
            if(this.IsUserSignedIn())
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Repositories/All");
            }

            var userId = usersService.GetUserId(username, password);

            if(userId == null)
            {
                return this.Error(GlobalConstants.LoginError);
            }

            this.SignIn(userId);

            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
