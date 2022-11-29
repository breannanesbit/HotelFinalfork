using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class Room
{
    public int Id { get; set; }

    public int RoomNumber { get; set; }

    public int RoomTypeId { get; set; }

    public virtual ICollection<RoomCleaning> RoomCleanings { get; } = new List<RoomCleaning>();

    public virtual RoomType RoomType { get; set; } = null!;
}
