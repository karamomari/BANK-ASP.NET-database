using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data;

public partial class BankContext : DbContext
{
    public BankContext()
    {
    }

    public BankContext(DbContextOptions<BankContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Atm> Atms { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Custmer> Custmers { get; set; }

    public virtual DbSet<CustmerPhone> CustmerPhones { get; set; }

    public virtual DbSet<CustomerMessage> CustomerMessages { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePhone> EmployeePhones { get; set; }

    public virtual DbSet<Help> Helps { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HP-VICTUS\\SQLEXPRESS;Database=bank;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__account__349DA586C2B304C5");

            entity.ToTable("account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BranchId).HasColumnName("branchID");
            entity.Property(e => e.CreditPoint).HasColumnName("credit_point");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.TypeAccount)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type_account");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Branch).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__account__branchI__1273C1CD");

            entity.HasMany(d => d.Ids).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "Crud",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__CRUD__ID__21B6055D"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__CRUD__AccountID__20C1E124"),
                    j =>
                    {
                        j.HasKey("AccountId", "Id").HasName("PK__CRUD__57BCEB448A4515E5");
                        j.ToTable("CRUD");
                        j.IndexerProperty<int>("AccountId").HasColumnName("AccountID");
                        j.IndexerProperty<int>("Id").HasColumnName("ID");
                    });
        });

        modelBuilder.Entity<Atm>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__ATM__55FECD8E0650D56F");

            entity.ToTable("ATM");

            entity.Property(e => e.CardId).HasColumnName("CardID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AtmCashLimt).HasColumnName("ATM_cash_Limt");
            entity.Property(e => e.Enable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("E");
            entity.Property(e => e.HolderName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("holder_name");

            entity.HasOne(d => d.Account).WithMany(p => p.Atms)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__ATM__AccountID__3C34F16F");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branch__751EBD3F978E88E3");

            entity.ToTable("Branch");

            entity.Property(e => e.BranchId).HasColumnName("branchID");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumberOfEmployee).HasColumnName("number_of_employee");
        });

        modelBuilder.Entity<Custmer>(entity =>
        {
            entity.HasKey(e => e.CustmerId).HasName("PK__custmer__497FDA0CDE75EA16");

            entity.ToTable("custmer");

            entity.HasIndex(e => e.Ssn, "UQ__custmer__DDDF0AE6185D9B9E").IsUnique();

            entity.Property(e => e.CustmerId).HasColumnName("custmer_id");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CreditPoint).HasColumnName("credit_point");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Enable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.LoginAccountId).HasColumnName("loginAccountID");
            entity.Property(e => e.NumForPas).HasColumnName("num_forPas");
            entity.Property(e => e.Pass)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.Photo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("photo");
            entity.Property(e => e.Ssn)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ssn");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("street");

            entity.HasOne(d => d.Account).WithMany(p => p.CustmerAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__custmer__Account__286302EC");

            entity.HasOne(d => d.LoginAccount).WithMany(p => p.CustmerLoginAccounts)
                .HasForeignKey(d => d.LoginAccountId)
                .HasConstraintName("FK__custmer__loginAc__29572725");
        });

        modelBuilder.Entity<CustmerPhone>(entity =>
        {
            entity.HasKey(e => new { e.Phone, e.CustmerId }).HasName("PK__custmer___BE3810A1DFC9EFD3");

            entity.ToTable("custmer__phone_");

            entity.Property(e => e.Phone)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("_phone_");
            entity.Property(e => e.CustmerId).HasColumnName("custmer_id");

            entity.HasOne(d => d.Custmer).WithMany(p => p.CustmerPhones)
                .HasForeignKey(d => d.CustmerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__custmer____custm__33D4B598");
        });

        modelBuilder.Entity<CustomerMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83FAEA445B9");

            entity.ToTable("customer_messages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("message");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerMessages)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__customer___custo__160F4887");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3214EC27D63131DA");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BranchId).HasColumnName("branchID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Enable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Pass)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.Photo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("photo");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.StartDate).HasColumnName("Start_Date");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.Account).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_");

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__employee__branch__182C9B23");
        });

        modelBuilder.Entity<EmployeePhone>(entity =>
        {
            entity.HasKey(e => new { e.Phone, e.Id }).HasName("PK__employee__D71A5A9CE5704558");

            entity.ToTable("employee_phone");

            entity.Property(e => e.Phone)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.EmployeePhones)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employee_pho__ID__24927208");
        });

        modelBuilder.Entity<Help>(entity =>
        {
            entity.HasKey(e => e.HlepNum).HasName("PK__help__885F695E86960D5C");

            entity.ToTable("help");

            entity.Property(e => e.HlepNum).HasColumnName("hlep_num");
            entity.Property(e => e.CustmerId).HasColumnName("custmer_id");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("message");

            entity.HasOne(d => d.Custmer).WithMany(p => p.Helps)
                .HasForeignKey(d => d.CustmerId)
                .HasConstraintName("FK__help__custmer_id__30F848ED");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Helps)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK__help__ID__300424B4");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("PK__loan__6DB7891FEEEE3AB2");

            entity.ToTable("loan");

            entity.Property(e => e.LoanId).HasColumnName("loanID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CustmerId).HasColumnName("custmer_id");
            entity.Property(e => e.Enable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Account).WithMany(p => p.Loans)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountID");

            entity.HasOne(d => d.Custmer).WithMany(p => p.Loans)
                .HasForeignKey(d => d.CustmerId)
                .HasConstraintName("FK_Customer_id");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.ProcessId).HasName("PK__process__01C9EBC2C61EB6F1");

            entity.ToTable("process");

            entity.Property(e => e.ProcessId).HasColumnName("processID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Tyoe)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tyoe");

            entity.HasOne(d => d.Account).WithMany(p => p.Processes)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__process__Account__1DE57479");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK___transac__9B57CF52532677EF");

            entity.ToTable("_transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.DepositeAccount).HasColumnName("Deposite_account_");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WithdrawalAccount).HasColumnName("withdrawal_account");

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK___transact__Accou__15502E78");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
