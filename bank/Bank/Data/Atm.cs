using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Atm
{
    public int CardId { get; set; }

    public string? HolderName { get; set; }

    public int? AtmCashLimt { get; set; }

    public int? AccountId { get; set; }

    public string? Enable { get; set; }

    public virtual Account? Account { get; set; }
}
