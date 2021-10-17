using CarShop.ViewModels.Cars;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface ICarsService
    {
        public string AddCar(string model, int year, string pictureUrl, string plateNumber, string ownerId);
        public IEnumerable<CarViewModel> GetCarsByUserId(string userId);
        public CarViewModel GetCarById(string carId);
        public IEnumerable<CarViewModel> GetAllCarsWithUnfixedIssues();
    }
}