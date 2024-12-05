using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Branch
{
    public string? Name { get; set; }

    public string? Location { get; set; }

    public int? NumberOfEmployee { get; set; }

    public int BranchId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
