using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class Reservation
{
    public int? Id { get; set; }

    public int GuestId { get; set; }

    public DateOnly ExpectedCheckin { get; set; }

    public DateOnly ExpectedCheckout { get; set; }

    public virtual Guest? Guest { get; set; } = null!;

    public virtual ICollection<Rental>? Rentals { get; } = new List<Rental>();

    public virtual ICollection<ReservationRoom>? ReservationRooms { get; } = new List<ReservationRoom>();
}
