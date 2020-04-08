using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class AddCarVM
    {
        [DisplayName("Type of car")]
        [Required(ErrorMessage = "Type of car is required")]
        public string CarType { get; set; }

        [DisplayName("Car LicenseNumber")]
        [Required(ErrorMessage = "The car licensenumber is required")]
        public string CarLicenseNumber { get; set; }

        [DisplayName("Current Mileage")]
        [Required]
        public int CurrentMileage { get; set; }
    }
}
