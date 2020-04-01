using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class RegisterBookingVM
    {
        public int BookingNumber { get; set; }
        [Required(ErrorMessage = "SSN is required")]
        [RegularExpression(@"^19\d{10}$", ErrorMessage = "Invalid Social Security Number")]
        public string ClientSSN { get; set; }
        [Required]
        public string CarType { get; set; }
        [Required]
        public string CarLicenseNumber { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TimeOfBooking { get; set; }
        [Required]
        public int CurrentMileage { get; set; }
        public bool Returned { get; set; }

    }
}
