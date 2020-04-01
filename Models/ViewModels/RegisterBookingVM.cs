using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class RegisterBookingVM
    {
        public int BookingNumber { get; set; }

        [DisplayName("SSN")]
        [Required(ErrorMessage = "SSN is required")]
        [RegularExpression(@"^(\d{10}|\d{12})$", ErrorMessage = "Invalid Social Security Number")]
        public string ClientSSN { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Type of car")]
        [Required]
        public string CarType { get; set; }

        [DisplayName("Licensenumber of car")]
        [Required]
        public string CarLicenseNumber { get; set; }

        [DisplayName("Time of Pickup")]
        [DataType(DataType.DateTime)]
        public DateTime TimeOfBooking { get; set; }

        [DisplayName("Current Mileage")]
        [Required]
        public int CurrentMileage { get; set; }

        public bool Returned { get; set; }

    }
}
