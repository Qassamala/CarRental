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
        }

        internal decimal TryGetCost(int id)
        {
            var booking = GetBookingById(id);

            var returnOfRental = GetReturnOfRentalCarById(id);

            decimal baseDayRental = 200;

            decimal kmPrice = 3;

            var numberOfDays = (TimeSpan)(returnOfRental.TimeOfReturn - booking.TimeOfBooking);

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

        public Booking GetBookingById(int id)
        {
            return context.Booking.Where(b => b.BookingNumber == id).FirstOrDefault();

            //var booking = bookings.Where(b => b.BookingNumber == bookingNumber).FirstOrDefault();

            //RegisterBookingVM bookingViewModel = new RegisterBookingVM
            //{
            //    BookingNumber = booking.BookingNumber,
            //    ClientSSN = booking.ClientSsn,
            //    CarType = booking.CarType,
            //    CarLicenseNumber = booking.CarLicenseNumber,
            //    TimeOfBooking = (DateTime)booking.TimeOfBooking,
            //    CurrentMileage = (int)booking.CurrentMileage
            //};

            //return bookingViewModel;
        }

        public ReturnOfRentalCar GetReturnOfRentalCarById(int id)
        {

            return context.ReturnOfRentalCar.Where(b => b.BookingNumber == id).FirstOrDefault();

            //var bookings = context.Booking.ToList();

            //var booking = bookings.Where(b => b.BookingNumber == bookingNumber).FirstOrDefault();

            //RegisterBookingVM bookingViewModel = new RegisterBookingVM
            //{
            //    BookingNumber = booking.BookingNumber,
            //    ClientSSN = booking.ClientSsn,
            //    CarType = booking.CarType,
            //    CarLicenseNumber = booking.CarLicenseNumber,
            //    TimeOfBooking = (DateTime)booking.TimeOfBooking,
            //    CurrentMileage = (int)booking.CurrentMileage
            //};

            //return bookingViewModel;
        }



        public RegisterBookingVM[] GetBookings()
        {

            return context.Booking
                .Select(b => new RegisterBookingVM
                {
                    BookingNumber = b.BookingNumber,
                    ClientSSN = b.ClientSsn,
                    CarType = b.CarType,
                    CarLicenseNumber = b.CarLicenseNumber,
                    TimeOfBooking = (DateTime)b.TimeOfBooking,
                    CurrentMileage = (int)b.CurrentMileage
                }).ToArray();

            //var bookings = context.Booking.ToList();

            //var booking = bookings.Where(b => b.BookingNumber == bookingNumber).FirstOrDefault();

            //RegisterBookingVM bookingViewModel = new RegisterBookingVM
            //{
            //    BookingNumber = booking.BookingNumber,
            //    ClientSSN = booking.ClientSsn,
            //    CarType = booking.CarType,
            //    CarLicenseNumber = booking.CarLicenseNumber,
            //    TimeOfBooking = (DateTime)booking.TimeOfBooking,
            //    CurrentMileage = (int)booking.CurrentMileage
            //};

            //return bookingViewModel;
        }
    }
}
