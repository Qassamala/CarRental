using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ViewModels
{
    public class ClientEventVM
    {
        [DisplayName("ClientSSN")]
        public string ClientSSN { get; set; }

        [DisplayName("LicenseNumber ")]
        public string CarLicenseNumber { get; set; }

        [DisplayName("Type of Event")]
        public string TypeOfEvent { get; set; }

        [DisplayName("Event description")]
        public string EventDescription { get; set; }

        [DisplayName("Time of Event")]
        [DataType(DataType.DateTime)]
        public DateTime TimeOfEvent { get; set; }
    }
}
