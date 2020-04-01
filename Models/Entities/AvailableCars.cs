﻿using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class AvailableCars
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public string CarLicenseNumber { get; set; }
        public int CurrentMileage { get; set; }
        public bool? IsAvailable { get; set; }
    }
}