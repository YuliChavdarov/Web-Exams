using CarShop.Data;
using CarShop.Data.Models;
using CarShop.ViewModels.Cars;
using CarShop.ViewModels.Issues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShop.Services
{
    public class CarsService : ICarsService
    {
        private readonly ApplicationDbContext context;

        public CarsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public string AddCar(string model, int year, string pictureUrl, string plateNumber, string ownerId)
        {
            var car = new Car
            {
                Id = Guid.NewGuid().ToString(),
                Model = model,
                Year = year,
                PictureUrl = pictureUrl,
                PlateNumber = plateNumber,
                OwnerId = ownerId
            };

            context.Cars.Add(car);
            context.SaveChanges();

            return car.Id;
        }

        public IEnumerable<CarViewModel> GetCarsByUserId(string userId)
        {
            return this.context.Cars
                .Where(x => x.OwnerId == userId)
                .Select(x => new CarViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Year = x.Year,
                    OwnerId = x.OwnerId,
                    PlateNumber = x.PlateNumber,
                    PictureUrl = x.PictureUrl,
                    FixedIssues = x.Issues.Count(x => x.IsFixed),
                    RemainingIssues = x.Issues.Count( x => x.IsFixed == false)
                })
                .ToList();
        }

        public CarViewModel GetCarById(string carId)
        {
            var car = this.context.Cars.Include(x => x.Issues).FirstOrDefault(x => x.Id == carId);

            if(car == null)
            {
                return null;
            }
            else
            {
                return new CarViewModel
                {
                    Id = car.Id,
                    Model = car.Model,
                    Year = car.Year,
                    OwnerId = car.OwnerId,
                    PlateNumber = car.PlateNumber,
                    PictureUrl = car.PictureUrl,
                    FixedIssues = car.Issues.Count(x => x.IsFixed),
                    RemainingIssues = car.Issues.Count(x => x.IsFixed == false),
                    Issues = car.Issues.Select(x => new IssueViewModel
                    {
                        CarId = car.Id,
                        Id = x.Id,
                        Description = x.Description,
                        IsFixed = x.IsFixed
                    })
                    .ToList()
                };
            }
        }

        public IEnumerable<CarViewModel> GetAllCarsWithUnfixedIssues()
        {
            return this.context.Cars
                .Where(x => x.Issues.Any(x => x.IsFixed == false))
                .Select(car => new CarViewModel
                {
                    Id = car.Id,
                    Model = car.Model,
                    Year = car.Year,
                    OwnerId = car.OwnerId,
                    PlateNumber = car.PlateNumber,
                    PictureUrl = car.PictureUrl,
                    FixedIssues = car.Issues.Count(x => x.IsFixed),
                    RemainingIssues = car.Issues.Count(x => x.IsFixed == false),
                    Issues = car.Issues.Select(x => new IssueViewModel
                    {
                        CarId = car.Id,
                        Id = x.Id,
                        Description = x.Description,
                        IsFixed = x.IsFixed
                    })
                    .ToList()
                })
                .ToList();
        }
    }
}
