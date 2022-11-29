using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class RoomServiceCharngeItem
{
    public int Id { get; set; }

    public int RoomServiceChargeId { get; set; }

    public int RoomServiceItemId { get; set; }

    public int Quanity { get; set; }

    public int ActualCost { get; set; }

    public virtual RoomServiceCharge RoomServiceCharge { get; set; } = null!;

    public virtual RoomServiceItem RoomServiceItem { get; set; } = null!;
}
