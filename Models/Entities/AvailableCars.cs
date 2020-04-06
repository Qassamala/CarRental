using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class AvailableCars
    {
        public AvailableCars()
        {
            CarEvents = new HashSet<CarEvents>();
            ClientEvents = new HashSet<ClientEvents>();
        }

        public int Id { get; set; }
        public string CarType { get; set; }
        public string CarLicenseNumber { get; set; }
        public int CurrentMileage { get; set; }
        public bool? IsAvailable { get; set; }
        public bool CleaningRequired { get; set; }
        public bool ServiceRequired { get; set; }
        public int TimesRented { get; set; }

        public virtual ICollection<CarEvents> CarEvents { get; set; }
        public virtual ICollection<ClientEvents> ClientEvents { get; set; }
    }
}
