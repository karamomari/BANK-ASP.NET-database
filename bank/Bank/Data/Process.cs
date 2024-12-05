using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Process
{
    public string? Tyoe { get; set; }

    public int ProcessId { get; set; }

    public DateTime? Date { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
