using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class RegisterReturnVM
    {
        [DisplayName("Bookingnumber")]
        [Required(ErrorMessage = "Booking number is required")]
        public int BookingNumber { get; set; }

        [DisplayName("Time of Return")]
        [Required(ErrorMessage = "Time of return is required")]
        [DataType(DataType.DateTime)]
        public DateTime TimeOfReturn { get; set; }

        [DisplayName("Distance used")]
        [Required(ErrorMessage = "Distance used is required")]
        public int DistanceCovered { get; set; }
    }
}
