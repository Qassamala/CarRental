using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class ClientEvents
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public string EventDescription { get; set; }
        public DateTime TimeOfEvent { get; set; }
        public string TypeOfEvent { get; set; }

        public virtual AvailableCars Car { get; set; }
        public virtual Clients Client { get; set; }
    }
}
