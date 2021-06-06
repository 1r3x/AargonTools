using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AargonTools.Models
{
    public partial class TempDataDbContext : DbContext
    {
        public TempDataDbContext()
        {
        }

        public TempDataDbContext(DbContextOptions<TempDataDbContext> options)
            : base(options)
        {
        }

      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=nv-sqltest01.aai.local;Database=collect;User Id=stephen;Password=Arizona2020!;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<EmployeeInfo>(entity =>
            {
                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.AcctStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Address1).IsUnicode(false);

                entity.Property(e => e.Address2).IsUnicode(false);

                entity.Property(e => e.Administrator)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Callable)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CommissionOr)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Company)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Department).IsUnicode(false);

                entity.Property(e => e.EmergencyContact).IsUnicode(false);

                entity.Property(e => e.EmployeeType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.HomePhone).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Mailable)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Payable)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Position).IsUnicode(false);

                entity.Property(e => e.Reportable)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SpokenLanguage).IsUnicode(false);

                entity.Property(e => e.Ssn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StateCode).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMaster>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered()
                    .HasFillFactor((byte)40);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMasterD>(entity =>
            {
                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered();

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMasterH>(entity =>
            {
                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered();

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMasterL>(entity =>
            {
                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered();

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMasterT>(entity =>
            {
                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered()
                    .HasFillFactor((byte)40);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMasterW>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .HasFillFactor((byte)35);

                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered()
                    .HasFillFactor((byte)40);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
