using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class Staff
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; } = new List<Rental>();

    public virtual ICollection<RoomCleaning> RoomCleanings { get; } = new List<RoomCleaning>();
}
