using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class ReservationRoom
{
    public int Id { get; set; }

    public int ReservationId { get; set; }

    public int RoomTypeId { get; set; }

    public virtual Reservation Reservation { get; set; } = null!;

    public virtual RoomType RoomType { get; set; } = null!;
}
