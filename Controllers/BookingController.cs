using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Models;
using CarRental.Models.Entities;
using CarRental.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingService service;

        public BookingController(BookingService service)
        {
            this.service = service;
        }

        [Route("/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("")]
        [Route("/Register")]
        public IActionResult Register()
        {
            var availableCars = service.TryGetCars();

            ViewData["AvailableCars"] = availableCars;
            ViewBag.AvailableCars = availableCars;

            return View();
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register(RegisterBookingVM registerBooking)
        {
            if (!ModelState.IsValid)
            {
                var availableCars = service.TryGetCars();
                ViewData["AvailableCars"] = availableCars;
                ViewBag.AvailableCars = availableCars;
                return View(registerBooking);
            }

            await service.TryRegisterBookingAsync(registerBooking);

            return RedirectToAction(nameof(Register));
        }

        [HttpGet]
        [Route("/Bookings")]
        public IActionResult GetBookings()
        {
            var bookings = service.GetBookings();

            return View(bookings);
        }


        [HttpGet]
        [Route("/Return/{id}")]
        public IActionResult Return(int id)
        {
            return View(new RegisterReturnVM {BookingNumber = id });
        }

        [HttpPost]
        [Route("/Return/{id}")]
        public async Task<IActionResult> Return(RegisterReturnVM registerReturn)
        {
            if (!ModelState.IsValid)
                return View(registerReturn);

            await service.TryRegisterReturnAsync(registerReturn);

            return RedirectToAction(nameof(GetCost), new { id = registerReturn.BookingNumber });
        }

        [HttpGet]
        [Route("/GetCost/{id}")]
        public IActionResult GetCost(int id)
        {
            decimal cost = service.TryGetCost(id);

            ViewBag.Message = $"Total cost of rental is {cost} SEK";

            return View();
        }

        [HttpGet]
        [Route("/getcars")]
        public IActionResult Getcars()
        {
            //var availableCars = service.TryGetCars();

            //ViewBag.AvailableCars = availableCars;

            return View();
        }

    }
}