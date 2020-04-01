using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class RegisterReturnVM
    {
        [Required(ErrorMessage = "Booking number is required")]
        public int BookingNumber { get; set; }
        [Required(ErrorMessage = "Time of return is required")]
        public DateTime TimeOfReturn { get; set; }
        [Required(ErrorMessage = "Distance used is required")]
        public int DistanceCovered { get; set; }
    }
}
