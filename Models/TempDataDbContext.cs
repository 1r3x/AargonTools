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

        public virtual DbSet<CcPayment> CcPayments { get; set; }

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

            modelBuilder.Entity<CcPayment>(entity =>
            {
                entity.Property(e => e.ApprovalCode).IsUnicode(false);

                entity.Property(e => e.ApprovalStatus).IsUnicode(false);

                entity.Property(e => e.BillingAddress1).IsUnicode(false);

                entity.Property(e => e.BillingAddress2).IsUnicode(false);

                entity.Property(e => e.BillingAreaCode).IsUnicode(false);

                entity.Property(e => e.BillingCity).IsUnicode(false);

                entity.Property(e => e.BillingName).IsUnicode(false);

                entity.Property(e => e.BillingPhone).IsUnicode(false);

                entity.Property(e => e.BillingState)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BillingZip).IsUnicode(false);

                entity.Property(e => e.CardCvv).IsUnicode(false);

                entity.Property(e => e.CardExpMonth)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CardExpYear)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CardNum).IsUnicode(false);

                entity.Property(e => e.CardType).IsUnicode(false);

                entity.Property(e => e.CbrFee).HasDefaultValueSql("((0))");

                entity.Property(e => e.Company).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.DebtorAddress1).IsUnicode(false);

                entity.Property(e => e.DebtorAddress2).IsUnicode(false);

                entity.Property(e => e.DebtorAreaCode).IsUnicode(false);

                entity.Property(e => e.DebtorCity).IsUnicode(false);

                entity.Property(e => e.DebtorName).IsUnicode(false);

                entity.Property(e => e.DebtorPhone).IsUnicode(false);

                entity.Property(e => e.DebtorState)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DebtorZip).IsUnicode(false);

                entity.Property(e => e.ErrorCode).IsUnicode(false);

                entity.Property(e => e.Hsa)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrderNumber).IsUnicode(false);

                entity.Property(e => e.RefNumber).IsUnicode(false);

                entity.Property(e => e.Sif)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.SmsDecline)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);

                entity.Property(e => e.VoidSale)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
