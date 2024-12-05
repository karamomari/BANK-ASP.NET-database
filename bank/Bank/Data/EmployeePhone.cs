using System;
using System.Collections.Generic;

namespace Bank.Data;

public partial class EmployeePhone
{
    public string Phone { get; set; } = null!;

    public int Id { get; set; }

    public virtual Employee IdNavigation { get; set; } = null!;
}
