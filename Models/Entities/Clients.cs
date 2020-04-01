using System;
using System.Collections.Generic;

namespace CarRental.Models.Entities
{
    public partial class Clients
    {
        public Clients()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClientSsn { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
    }
}
