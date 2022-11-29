using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class Guest
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; } = new List<Rental>();

    public virtual ICollection<Reservation> Reservations { get; } = new List<Reservation>();
}
