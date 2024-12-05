using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Loan
{
    public int LoanId { get; set; }

    public string? Enable { get; set; }

    public int? AccountId { get; set; }

    public int? CustmerId { get; set; }

    public int? Amount { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Custmer? Custmer { get; set; }
}
