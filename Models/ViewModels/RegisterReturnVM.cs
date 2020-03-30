using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class RegisterReturnVM
    {
        public int BookingNumber { get; set; }
        public DateTime TimeOfReturn { get; set; }
        public int DistanceCovered { get; set; }
    }
}
