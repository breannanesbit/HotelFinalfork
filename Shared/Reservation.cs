using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class Reservation
{
    public int? Id { get; set; }

    public int GuestId { get; set; }

    public DateOnly ExpectedCheckin { get; set; }

    public DateOnly ExpectedCheckout { get; set; }

    public virtual Guest? Guest { get; set; }

    public virtual ICollection<Rental>? Rentals { get; } 

    public virtual ICollection<ReservationRoom>? ReservationRooms { get; } 
}
