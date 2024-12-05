using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Custmer
{
    public int CustmerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Ssn { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public int? Age { get; set; }

    public string? Email { get; set; }

    public string? Pass { get; set; }

    public double? CreditPoint { get; set; }

    public string? Enable { get; set; }

    public string? Photo { get; set; }

    public int? AccountId { get; set; }

    public int? LoginAccountId { get; set; }

    public int? NumForPas { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<CustmerPhone> CustmerPhones { get; set; } = new List<CustmerPhone>();

    public virtual ICollection<CustomerMessage> CustomerMessages { get; set; } = new List<CustomerMessage>();

    public virtual ICollection<Help> Helps { get; set; } = new List<Help>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual Account? LoginAccount { get; set; }
}
