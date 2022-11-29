using System;
using System.Collections.Generic;

namespace HotelFinal.Shared;

public partial class RentalPayment
{
    public int Id { get; set; }

    public int RentalId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public virtual Rental Rental { get; set; } = null!;
}
