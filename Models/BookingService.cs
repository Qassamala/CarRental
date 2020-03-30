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
