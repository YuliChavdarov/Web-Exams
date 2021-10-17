using CarShop.Services;
using CarShop.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Cars/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Cars/All");
            }

            var userId = usersService.GetUserId(username, password);

            if (userId == null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);

            return this.Redirect("/Cars/All");
        }

        public HttpResponse Logout()
        {
            if(!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.SignOut();

            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Cars/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel inputModel)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Cars/All");
            }

            if (inputModel.Username == null || inputModel.Username.Length < 4 || inputModel.Username.Length > 20)
            {
                return this.Error("Username length must be between 4 and 20 characters.");
            }

            if(usersService.IsUsernameAvailable(inputModel.Username) == false)
            {
                return this.Error("This username is not available.");
            }

            if (inputModel.Email == null || new EmailAddressAttribute().IsValid(inputModel.Email) == false)
            {
                return this.Error("Invalid email.");
            }

            if(inputModel.Password == null || inputModel.Password.Length < 5 || inputModel.Password.Length > 20)
            {
                return this.Error("Password length must be between 5 and 20 characters.");
            }

            if (inputModel.ConfirmPassword == null || inputModel.ConfirmPassword != inputModel.Password)
            {
                return this.Error("Confirm password does not match.");
            }

            bool isMechanic;

            if(inputModel.UserType == "Client")
            {
                isMechanic = false;
            }
            else if (inputModel.UserType == "Mechanic")
            {
                isMechanic = true;
            }
            else
            {
                return this.Error("Invalid user type.");
            }

            usersService.Create(inputModel.Username, inputModel.Email, inputModel.Password, isMechanic);

            return this.Redirect("/Users/Login");
        }
    }
}
