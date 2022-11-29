using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class CleaningType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<RoomCleaning> RoomCleanings { get; } = new List<RoomCleaning>();
}
