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
            var client = GetClient(viewModel.ClientSSN);

            if (client == null)
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

            var car = GetCar(viewModel.CarLicenseNumber);

            var clientData = GetClient(viewModel.ClientSSN);

            context.Add(new ClientEvents
            {
                ClientId = clientData.Id,
                CarId = car.Id,
                EventDescription = $"{clientData.FirstName} {clientData.LastName} picked up car {car.CarLicenseNumber}.",
                TimeOfEvent = DateTime.Now,
                TypeOfEvent = "Pickup",
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

        public Clients GetClient(string clientSSN)
        {
            var client = context.Clients.Where(c => c.ClientSsn == clientSSN).FirstOrDefault();

            return client;
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

            car.TimesRented += 1;

            context.AvailableCars.Update(car);

            var client = GetClient(booking.ClientSsn);

            client.DistanceCovered += viewModel.DistanceCovered;

            context.Clients.Update(client);


            context.Add(new ClientEvents
            {
                ClientId = client.Id,
                CarId = car.Id,
                EventDescription = $"{client.FirstName} {client.LastName} returned car {car.CarLicenseNumber}.",
                TimeOfEvent = DateTime.Now,
                TypeOfEvent = "Return",
            });

            // Save current mileage on car, times rented, distance covered for client and client event
            await context.SaveChangesAsync();

            //Set client loyalty level
            var numberOfRentals = context.ClientEvents.Where(c => c.ClientId == client.Id && c.TypeOfEvent == "Return").Count();

            switch (numberOfRentals)
            {
                case int n when (numberOfRentals >= 3 && numberOfRentals < 5) && client.LoyaltyLevel != "Bronze":
                    client.LoyaltyLevel = "Bronze";
                    context.Clients.Update(client);
                    SetClientEventLoyalty(client, car);
                    break;

                case int n when (numberOfRentals >= 5 && client.DistanceCovered < 1000) && client.LoyaltyLevel != "Silver":
                    client.LoyaltyLevel = "Silver";
                    context.Clients.Update(client);
                    SetClientEventLoyalty(client, car);
                    break;

                case int n when (numberOfRentals >= 5 && client.DistanceCovered >= 1000) && client.LoyaltyLevel != "Gold":
                    client.LoyaltyLevel = "Gold";
                    context.Clients.Update(client);
                    SetClientEventLoyalty(client, car);
                    break;
                default:
                    break;
            }

            if (car.CurrentMileage <= 2000)
            {
                // Cleaning is required everytime after return
                car.CleaningRequired = true;
                context.AvailableCars.Update(car);

                // Service is required after every third rental
                if (car.TimesRented % 3 == 0)
                {
                    car.ServiceRequired = true;
                    context.AvailableCars.Update(car);
                }
            }
            // Save cleaning required, service required and loyalty level
            await context.SaveChangesAsync();
        }

        public void SetClientEventLoyalty(Clients client, AvailableCars car)
        {
            context.Add(new ClientEvents
            {
                ClientId = client.Id,
                CarId = car.Id,
                EventDescription = $"{client.FirstName} {client.LastName} was promotod to loyalty level {client.LoyaltyLevel}.",
                TimeOfEvent = DateTime.Now,
                TypeOfEvent = "LoyaltyPromotion",
            });
        }

        public async Task SetCarIsCleaned(string carLicenseNumber)
        {
            var car = GetCar(carLicenseNumber);

            context.Add(new CarEvents
            {
                CarId = car.Id,
                EventDescription = $"Cleaning done on car {car.CarLicenseNumber}.",
                TimeOfEvent = DateTime.Now,
                TypeOfEvent = "Cleaned",
            });

            car.CleaningRequired = false;

            context.AvailableCars.Update(car);
            await context.SaveChangesAsync();

            // Set car is available only if no cleaning and service is required
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

            context.Add(new CarEvents
            {
                CarId = car.Id,
                EventDescription = $"Service done on car {car.CarLicenseNumber}.",
                TimeOfEvent = DateTime.Now,
                TypeOfEvent = "Serviced",
            });
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

            var client = GetClient(booking.ClientSsn);

            decimal baseDayRental = 200;

            decimal kmPrice = 3;

            // If car is returned within 24 hours, one day is still charged, if car is returned after 24 hours, 2 days are charged
            var numberOfDays = 1 + (returnOfRental.TimeOfReturn - booking.TimeOfBooking).Days;

            var numberOfKilometers = returnOfRental.DistanceCovered;

            switch (client.LoyaltyLevel)
            {
                case "Bronze":
                    baseDayRental = baseDayRental/2;
                    break;

                case "Silver":
                    baseDayRental = baseDayRental/2;
                    if (numberOfDays == 3 || numberOfDays == 4)
                    {
                        numberOfDays = 2;
                    }
                    else if (numberOfDays > 4)
                    {
                        numberOfDays = numberOfDays - 2;
                    }
                    break;

                case "Gold":
                    baseDayRental = baseDayRental / 2;
                    if (numberOfDays == 3 || numberOfDays == 4)
                    {
                        numberOfDays = 2;
                    }
                    else if (numberOfDays > 4)
                    {
                        numberOfDays = numberOfDays - 2;
                    }

                    if (numberOfKilometers <= 20)
                    {
                        numberOfKilometers = 0;
                    }
                    else if (numberOfKilometers > 20)
                    {
                        numberOfKilometers = numberOfKilometers - 20;
                    }
                    break;
                default:
                    break;
            }

            decimal finalPrice = 0;

            switch (booking.CarType)
            {
                case "Small car":
                    finalPrice = baseDayRental * (numberOfDays);
                    break;

                case "Van":
                    finalPrice = baseDayRental * (numberOfDays) * (decimal)1.2 + kmPrice * numberOfKilometers;
                    break;

                case "Minibus":
                    finalPrice = baseDayRental * (numberOfDays) * (decimal)1.7 + kmPrice * numberOfKilometers;
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
