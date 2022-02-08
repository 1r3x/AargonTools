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
                optionsBuilder.UseSqlServer("Server=192.168.0.10;Database=collect;User Id=cfusion;Password=T3mp@cc3ss;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CheckDetail>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsClustered();

                entity.Property(e => e.AddAdminFee)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BusinessCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.Micr).IsUnicode(false);

                entity.Property(e => e.Payee).IsUnicode(false);

                entity.Property(e => e.PayorAddress).IsUnicode(false);

                entity.Property(e => e.PayorCity).IsUnicode(false);

                entity.Property(e => e.PayorFirstName).IsUnicode(false);

                entity.Property(e => e.PayorLastName).IsUnicode(false);

                entity.Property(e => e.PayorPhone).IsUnicode(false);

                entity.Property(e => e.PayorState).IsUnicode(false);

                entity.Property(e => e.PayorZip).IsUnicode(false);

                entity.Property(e => e.Sif)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorPpInfo>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.How).IsUnicode(false);

                entity.Property(e => e.Monthly1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Monthly2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PendingDp)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PromiseType).IsUnicode(false);

                entity.Property(e => e.What).IsUnicode(false);

                entity.Property(e => e.Where).IsUnicode(false);
            });

            modelBuilder.Entity<LarryCcPayment>(entity =>
            {
                entity.Property(e => e.Address1).IsUnicode(false);

                entity.Property(e => e.Address2).IsUnicode(false);

                entity.Property(e => e.ApprovalCode).IsUnicode(false);

                entity.Property(e => e.Batch).IsUnicode(false);

                entity.Property(e => e.CardHolder).IsUnicode(false);

                entity.Property(e => e.CardNum).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Company).IsUnicode(false);

                entity.Property(e => e.Confirmed).IsUnicode(false);

                entity.Property(e => e.Cvv).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.ExpMonth).IsUnicode(false);

                entity.Property(e => e.ExpYear).IsUnicode(false);

                entity.Property(e => e.Hsa)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.NextAcct).IsUnicode(false);

                entity.Property(e => e.Processed)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Results).IsUnicode(false);

                entity.Property(e => e.SmsNotified)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.StateCode).IsUnicode(false);

                entity.Property(e => e.Status).IsUnicode(false);

                entity.Property(e => e.Verified)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Zip).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
