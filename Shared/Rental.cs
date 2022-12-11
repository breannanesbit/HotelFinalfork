using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class Rental
{
    public int? Id { get; set; }

    public int? ReservationId { get; set; }

    public int StaffId { get; set; }

    public int GuestId { get; set; }

    public DateOnly Checkin { get; set; }

    public DateOnly? Checkout { get; set; }

    public virtual Guest? Guest { get; set; } = null!;

    public virtual ICollection<RentalPayment>? RentalPayments { get; } = new List<RentalPayment>();

    public virtual ICollection<RentalRoom>? RentalRooms { get; } = new List<RentalRoom>();

    public virtual Reservation? Reservation { get; set; }

    public virtual Staff? Staff { get; set; } = null!;
}
