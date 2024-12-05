using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Transaction
{
    public string? Type { get; set; }

    public int? DepositeAccount { get; set; }

    public DateOnly? Date { get; set; }

    public int? WithdrawalAccount { get; set; }

    public int TransactionId { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
