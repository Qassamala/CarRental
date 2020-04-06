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

        internal ClientEventVM[] GetEventsByClient(int id)
        {            
            return context.ClientEvents
                .Where(e => e.ClientId == id)
                .Select(e => new ClientEventVM
                {
                    ClientSSN = context.Clients.Where(c => c.Id == id).Select(c => c.ClientSsn).FirstOrDefault(),
                    CarLicenseNumber = context.AvailableCars.Where(c => c.Id == e.CarId).Select(car => car.CarLicenseNumber).FirstOrDefault(),
                    EventDescription = e.EventDescription,
                    TypeOfEvent = e.TypeOfEvent,
                    TimeOfEvent = e.TimeOfEvent
                })
                .ToArray();
        }

        internal CarEventVM[] GetEventsByCar(int carId)
        {
                    return context.CarEvents
            .Where(e => e.CarId == carId)
            .Select(e => new CarEventVM
            {
                CarLicenseNumber = context.AvailableCars.Where(c => c.Id == e.CarId).Select(car => car.CarLicenseNumber).FirstOrDefault(),
                EventDescription = e.EventDescription,
                TypeOfEvent = e.TypeOfEvent,
                TimeOfEvent = e.TimeOfEvent
            })
            .ToArray();
        }

        internal bool CheckIfCarExists(AddCarVM newCar)
        {
            var car = context.AvailableCars.Any(c => c.CarLicenseNumber == newCar.CarLicenseNumber);

            return car ? true : false;
        }
    }
}
