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
    public class CarController : Controller
    {
        private readonly CarService service;

        public CarController(CarService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("/ClientEvents/{id}")]
        public IActionResult ClientEvents(int id)
        {
            var events = service.GetEventsByClient(id);

            return View(events);
        }

        [HttpGet]
        [Route("/CarEvents/{id}")]
        public IActionResult CarEvents(int id)
        {

            var events = service.GetEventsByCar(id);

            return View(events);
        }

        [HttpPost]
        [Route("/Create")]
        public async Task<IActionResult> Create(AddCarVM newCar)
        {
            if (!ModelState.IsValid)
            {
                return View(newCar);
            }

            bool carExists  = service.CheckIfCarExists(newCar);

            if (carExists)
            {
                ModelState.AddModelError(nameof(AddCarVM.CarLicenseNumber), $"{newCar.CarLicenseNumber} already exists.");
                return View(newCar);
            }
            else
            {
                await service.TryRegisterCarAsync(newCar);
            }

            return RedirectToAction("Cars", "Booking");
        }

        [HttpGet]
        [Route("/GetCar/{id}")]
        public IActionResult GetCar(int id)
        {
            var car = service.TryGetCarById(id);

            return Json(car);
        }

        [HttpGet]
        [Route("/getcars")]
        public IActionResult GetCars()
        {
            var availableCars = service.TryGetAvailableCars();

            return Json(availableCars);
        }
    }
}