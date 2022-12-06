using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class RoomType
{
    public int Id { get; set; }

    public string RType { get; set; } = null!;

    public decimal BaseRentalRate { get; set; }

    public bool Smoking { get; set; }
}
