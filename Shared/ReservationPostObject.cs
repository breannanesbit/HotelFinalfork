using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinal.Shared
{
    public class ReservationPostObject
    {
        public Reservation Reservation { get; set; }
        public List<RoomType> RoomTypes { get; set; }
    }
}
