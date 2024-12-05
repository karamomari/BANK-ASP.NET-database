using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Help
{
    public int HlepNum { get; set; }

    public int? Id { get; set; }

    public int? CustmerId { get; set; }

    public string? Message { get; set; }

    public virtual Custmer? Custmer { get; set; }

    public virtual Employee? IdNavigation { get; set; }
}
