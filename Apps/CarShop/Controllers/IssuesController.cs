using CarShop.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarShop.Controllers
{
    public class IssuesController : Controller
    {
        private readonly ICarsService carsService;
        private readonly IIssuesService issuesService;
        private readonly IUsersService usersService;

        public IssuesController(ICarsService carsService, IIssuesService issuesService, IUsersService usersService)
        {
            this.carsService = carsService;
            this.issuesService = issuesService;
            this.usersService = usersService;
        }

        public HttpResponse CarIssues(string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var carViewModel = carsService.GetCarById(carId);

            if (carViewModel == null)
            {
                return this.Error("Car not found.");
            }

            return this.View(carViewModel);
        }


        public HttpResponse Add(string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var car = carsService.GetCarById(carId);
            if (car == null)
            {
                return this.Error("Car not found.");
            }

            if (usersService.isMechanic(this.GetUserId()) == false && car.OwnerId != this.GetUserId())
            {
                return this.Error("Only mechanics and the owner of a car can create an issue for it.");
            }

            return this.View(carId);
        }

        [HttpPost]
        public HttpResponse Add(string carId, string description)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (description.Length < 5)
            {
                return this.Error("Description length must be at least 5 characters.");
            }

            issuesService.CreateIssue(carId, description);

            return this.Redirect($"/Issues/CarIssues?carId={carId}");
        }

        public HttpResponse Delete(string issueId, string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var car = carsService.GetCarById(carId);

            if (car == null)
            {
                return this.Error("Car not found.");
            }

            if(usersService.isMechanic(this.GetUserId()) == false && car.OwnerId != this.GetUserId())
            {
                return this.Error("Only mechanics and the owner of a car can delete its issues.");
            }

            issuesService.DeleteIssue(issueId);

            return this.Redirect($"/Issues/CarIssues?carId={carId}");
        }

        public HttpResponse Fix(string issueId, string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(usersService.isMechanic(this.GetUserId()) == false)
            {
                return this.Error("Only mechanics can fix issues.");
            }

            issuesService.FixIssue(issueId);

            return this.Redirect($"/Issues/CarIssues?carId={carId}");
        }
    }
}
