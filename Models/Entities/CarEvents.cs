using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class CarEvents
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string EventDescription { get; set; }
        public DateTime TimeOfEvent { get; set; }
        public string TypeOfEvent { get; set; }

        public virtual AvailableCars Car { get; set; }
    }
}
