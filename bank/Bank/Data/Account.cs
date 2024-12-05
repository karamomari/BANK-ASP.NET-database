using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Account
{
    public string? TypeAccount { get; set; }

    public int? CreditPoint { get; set; }

    public double? Balance { get; set; }

    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int? BranchId { get; set; }

    public virtual ICollection<Atm> Atms { get; set; } = new List<Atm>();

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<Custmer> CustmerAccounts { get; set; } = new List<Custmer>();

    public virtual ICollection<Custmer> CustmerLoginAccounts { get; set; } = new List<Custmer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Employee> Ids { get; set; } = new List<Employee>();
}
