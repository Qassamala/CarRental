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
        [Required]
        public string CarType { get; set; }

        [DisplayName("Car LicenseNumber")]
        [Required]
        public string CarLicenseNumber { get; set; }

        [DisplayName("Current Mileage")]
        [Required]
        public int CurrentMileage { get; set; }
    }
}
