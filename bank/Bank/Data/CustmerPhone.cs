using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class CustmerPhone
{
    public string Phone { get; set; } = null!;

    public int CustmerId { get; set; }

    public virtual Custmer Custmer { get; set; } = null!;
}
