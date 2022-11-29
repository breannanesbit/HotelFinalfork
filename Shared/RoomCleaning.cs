using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class RoomCleaning
{
    public int Id { get; set; }

    public DateOnly DateCleaned { get; set; }

    public int CleaningTypeId { get; set; }

    public int RoomId { get; set; }

    public int StaffId { get; set; }

    public virtual CleaningType CleaningType { get; set; } = null!;

    public virtual RentalRoom? RentalRoom { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
