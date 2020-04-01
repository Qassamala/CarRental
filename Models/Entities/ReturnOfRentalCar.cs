using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class ReturnOfRentalCar
    {
        public int Id { get; set; }
        public int BookingNumber { get; set; }
        public int DistanceCovered { get; set; }
        public DateTime TimeOfReturn { get; set; }

        public virtual Booking BookingNumberNavigation { get; set; }
    }
}
