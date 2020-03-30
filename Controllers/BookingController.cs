using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Models;
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
            return View();
        }

        [HttpPost]
        [Route("/Register")]
        public IActionResult Register(RegisterBookingVM registerBooking)
        {
            if (!ModelState.IsValid)
                return View(registerBooking);

            var result = service.TryRegisterBookingAsync(registerBooking);

            return RedirectToAction(nameof(Register));
        }

        [HttpGet]
        [Route("/Return")]
        public IActionResult Return()
        {
            return View();
        }

        [HttpPost]
        [Route("/Return")]
        public IActionResult Return(RegisterReturnVM registerReturn)
        {
            //if (!ModelState.IsValid)
            //    return View(registerReturn);

            //var result = service.TryRegisterBookingAsync(registerReturn);

            return RedirectToAction(nameof(GetCost));
        }

        [HttpGet]
        [Route("/GetCost")]
        public IActionResult GetCost()
        {
            return View();
        }
    }
}