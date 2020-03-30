using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class Booking
    {
        public int BookingNumber { get; set; }
        public string ClientSsn { get; set; }
        public string CarType { get; set; }
        public string CarLicenseNumber { get; set; }
        public DateTime? TimeOfBooking { get; set; }
        public int? CurrentMileage { get; set; }
    }
}
