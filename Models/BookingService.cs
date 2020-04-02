using CarRental.Models.Entities;
using CarRental.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class BookingService
    {
        private readonly CarRentalContext context;

        public BookingService(CarRentalContext context)
        {
            this.context = context;
        }
        public async Task TryRegisterBookingAsync(RegisterBookingVM viewModel)
        {
            var client = context.Clients.Any(c => c.ClientSsn == viewModel.ClientSSN);

            if (client == false)
            {
                context.Add(new Clients {
                    ClientSsn= viewModel.ClientSSN,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                });

                await context.SaveChangesAsync();
            }

            context.Add(new Booking
            {
                BookingNumber = viewModel.BookingNumber,
                ClientSsn = viewModel.ClientSSN,
                CarType = viewModel.CarType,
                CarLicenseNumber = viewModel.CarLicenseNumber,
                TimeOfBooking = viewModel.TimeOfBooking,
                CurrentMileage = viewModel.CurrentMileage
            });

            await context.SaveChangesAsync();

            await SetIsNotAvailable(viewModel.CarLicenseNumber);
        }

        internal Clients[] TryGetAllClients()
        {
            return context.Clients.ToArray();
        }

        public AvailableCars GetCar(string carLicenseNumber)
        {
            var car = context.AvailableCars.Where(c => c.CarLicenseNumber == carLicenseNumber).FirstOrDefault();

            return car;
        }

        public async Task SetIsNotAvailable(string carLicenseNumber)
        {
            var car = GetCar(carLicenseNumber);

            car.IsAvailable = false;

            context.AvailableCars.Update(car);

            await context.SaveChangesAsync();
        }

        public async Task SetIsAvailable(string carLicenseNumber)
        {
            var car = GetCar(carLicenseNumber);

            car.IsAvailable = true;

            context.AvailableCars.Update(car);

            await context.SaveChangesAsync();
        }

        public async Task TryRegisterReturnAsync(RegisterReturnVM viewModel)
        {
            context.Add(new ReturnOfRentalCar
            {
                BookingNumber = viewModel.BookingNumber,
                TimeOfReturn = viewModel.TimeOfReturn,
                DistanceCovered = viewModel.DistanceCovered
            });
            await context.SaveChangesAsync();            

            var booking = GetBookingById(viewModel.BookingNumber);

            booking.Returned = true;

            context.Booking.Update(booking);

            var car = GetCar(booking.CarLicenseNumber);

            car.CurrentMileage += viewModel.DistanceCovered;

            // Cleaning is required everytime after return
            car.CleaningRequired = true;

            car.TimesRented += 1;

            context.AvailableCars.Update(car);

            // Save current mileage on car, times rented and cleaning required
            await context.SaveChangesAsync();

            // Service is required after every third rental
            if (car.TimesRented % 3 == 0)
            {
                car.ServiceRequired = true;
                context.AvailableCars.Update(car);
                await context.SaveChangesAsync();
            }
        }

        public async Task SetCarIsCleaned(string carLicenseNumber)
        {
            var car = GetCar(carLicenseNumber);

            car.CleaningRequired = false;

            context.AvailableCars.Update(car);
            await context.SaveChangesAsync();

            // Set car i available only if no cleaning and service is required
            if (!car.CleaningRequired && !car.ServiceRequired)
            {
                await SetIsAvailable(carLicenseNumber);
            }
        }

        public async Task SetCarIsServiced(string carLicenseNumber)
        {
            var car = GetCar(carLicenseNumber);

            car.ServiceRequired = false;

            context.AvailableCars.Update(car);
            await context.SaveChangesAsync();

            // Set car is available only if no cleaning and service is required
            if (!car.CleaningRequired && !car.ServiceRequired)
            {
                await SetIsAvailable(carLicenseNumber);
            }
        }

        internal decimal TryGetCost(int id)
        {
            var booking = GetBookingById(id);

            var returnOfRental = GetReturnOfRentalCarById(id);

            decimal baseDayRental = 200;

            decimal kmPrice = 3;

            var numberOfDays = (returnOfRental.TimeOfReturn - booking.TimeOfBooking);

            var numberOfKilometers = returnOfRental.DistanceCovered;

            decimal finalPrice = 0;

            switch (booking.CarType)
            {
                case "Small car":
                    finalPrice = baseDayRental * (1 + numberOfDays.Days);
                    break;

                case "Van":
                    finalPrice = baseDayRental * (1 + numberOfDays.Days) * (decimal)1.2 + kmPrice * numberOfKilometers;
                    break;

                case "Minibus":
                    finalPrice = baseDayRental * (1 + numberOfDays.Days) * (decimal)1.7 + kmPrice * numberOfKilometers;
                    break;

                default:
                    break;
            }

            return finalPrice;

        }

        internal AvailableCars[] TryGetAvailableCars()
        {
            return context.AvailableCars.Where(c => c.IsAvailable == true).ToArray();
        }

        internal AvailableCars[] TryGetAllCars()
        {
            return context.AvailableCars.ToArray();
        }

        public Booking GetBookingById(int id)
        {
            return context.Booking.Where(b => b.BookingNumber == id).FirstOrDefault();
        }

        public ReturnOfRentalCar GetReturnOfRentalCarById(int id)
        {
            return context.ReturnOfRentalCar.Where(b => b.BookingNumber == id).FirstOrDefault();
        }

        public RegisterBookingVM[] GetBookingsByClient(int id)
        {
            var clientSSN = context.Clients
                .Where(c => c.Id == id)
                .Select(c => c.ClientSsn)
                .FirstOrDefault();

            return context.Booking.Where(b => b.ClientSsn == clientSSN)
            .Select(b => new RegisterBookingVM
            {
                BookingNumber = b.BookingNumber,
                ClientSSN = b.ClientSsn,
                CarType = b.CarType,
                CarLicenseNumber = b.CarLicenseNumber,
                TimeOfBooking = b.TimeOfBooking,
                CurrentMileage = b.CurrentMileage,
                Returned = b.Returned
            }).ToArray();
        }



        public RegisterBookingVM[] GetBookings()
        {

        return context.Booking.Where(b=> b.Returned == false)
            .Select(b => new RegisterBookingVM
            {
                BookingNumber = b.BookingNumber,
                ClientSSN = b.ClientSsn,
                CarType = b.CarType,
                CarLicenseNumber = b.CarLicenseNumber,
                TimeOfBooking = b.TimeOfBooking,
                CurrentMileage = b.CurrentMileage,
                Returned = b.Returned
            }).ToArray();       
        }
    }
}
