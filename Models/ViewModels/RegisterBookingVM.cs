using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class RegisterBookingVM
    {
        public int BookingNumber { get; set; }
        public string ClientSSN { get; set; }
        public string CarType { get; set; }
        public string CarLicenseNumber { get; set; }
        public DateTime TimeOfPickUp { get; set; }
        public int CurrentMileage { get; set; }
    }
}
