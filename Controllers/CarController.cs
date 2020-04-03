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

            return RedirectToAction(nameof(Create));
        }
    }
}