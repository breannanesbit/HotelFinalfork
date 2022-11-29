using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class RoomCleaningInfo
{
    public int? RoomId { get; set; }

    public int? RoomNumber { get; set; }

    public string? RoomType { get; set; }

    public string? Staff { get; set; }

    public DateOnly? DateCleaned { get; set; }
}
