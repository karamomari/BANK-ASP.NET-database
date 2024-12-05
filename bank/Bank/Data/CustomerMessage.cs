using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class CustomerMessage
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public string? Message { get; set; }

    public virtual Custmer? Customer { get; set; }
}
