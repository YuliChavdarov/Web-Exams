using CarShop.Services;
using CarShop.ViewModels.Cars;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService carsService;
        private readonly IUsersService usersService;

        public CarsController(ICarsService carsService, IUsersService usersService)
        {
            this.carsService = carsService;
            this.usersService = usersService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(usersService.isClient(this.GetUserId()))
            {
                var clientViewModels = carsService.GetCarsByUserId(this.GetUserId());

                return this.View(clientViewModels);
            }

            else if(usersService.isMechanic(this.GetUserId()))
            {
                var mechanicViewModels = carsService.GetAllCarsWithUnfixedIssues();

                return this.View(mechanicViewModels);
            }
            else
            {
                return this.Error("View for current user type not implemented.");
            }
        }

        public HttpResponse Add()
        {
            if(!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(usersService.isMechanic(this.GetUserId()))
            {
                return this.Error("Mechanics cannot add new cars.");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(CarInputModel inputModel)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (inputModel.Model == null || inputModel.Model.Length < 5 || inputModel.Model.Length > 20)
            {
                return this.Error("Model length must be between 5 and 20 characters.");
            }

            int year = 0;

            if (inputModel.Year == null || int.TryParse(inputModel.Year, out year) == false || year < 1500)
            {
                return this.Error("Invalid year.");
            }

            if (inputModel.Image == null)
            {
                return this.Error("Invalid image url.");
            }

            if (inputModel.PlateNumber == null || !Regex.IsMatch(inputModel.PlateNumber, "^[A-Z]{2}[0-9]{4}[A-Z]{2}$"))
            {
                return this.Error("Invalid plate number.");
            }

            carsService.AddCar(inputModel.Model, year, inputModel.Image, inputModel.PlateNumber, this.GetUserId());

            return this.Redirect("/Cars/All");
        }
    }
}
