using CarRental.Models.Entities;
using CarRental.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class CarService
    {

        private readonly CarRentalContext context;

        public CarService(CarRentalContext context)
        {
            this.context = context;
        }
        internal async Task TryRegisterCarAsync(AddCarVM newCar)
        {

                context.Add(new AvailableCars
                {
                    CarType = newCar.CarType,
                    CarLicenseNumber = newCar.CarLicenseNumber,
                    CurrentMileage = newCar.CurrentMileage,
                });

                await context.SaveChangesAsync();
        }

        internal bool CheckIfCarExists(AddCarVM newCar)
        {
            var car = context.AvailableCars.Any(c => c.CarLicenseNumber == newCar.CarLicenseNumber);

            return car ? true : false;
        }
    }
}
