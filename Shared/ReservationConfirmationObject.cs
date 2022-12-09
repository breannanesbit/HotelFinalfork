using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinal.Shared
{
    public class ReservationConfirmationObject
    {
        public string UserEmail { get; set; }
        public string Checkin { get; set; }
        public string Checkout { get; set; }
    }
}
