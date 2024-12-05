using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class Employee
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int Id { get; set; }

    public double? Salary { get; set; }

    public DateTime? StartDate { get; set; }

    public string? Type { get; set; }

    public string? Email { get; set; }

    public string? Pass { get; set; }

    public string? Enable { get; set; }

    public string? Photo { get; set; }

    public int? BranchId { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<EmployeePhone> EmployeePhones { get; set; } = new List<EmployeePhone>();

    public virtual ICollection<Help> Helps { get; set; } = new List<Help>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
