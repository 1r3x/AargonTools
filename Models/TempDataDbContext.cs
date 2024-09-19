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

        public virtual DbSet<ClientAcctInfoP> ClientAcctInfoPs { get; set; }
        public virtual DbSet<ClientMasterP> ClientMasterPs { get; set; }
        public virtual DbSet<DebtorAcctInfoP> DebtorAcctInfoPs { get; set; }
        public virtual DbSet<DebtorMasterP> DebtorMasterPs { get; set; }
        public virtual DbSet<QueueMasterP> QueueMasterPs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=10.56.71.2;Database=collect;User Id=stephen;Password=Arizona2020!;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ClientAcctInfoP>(entity =>
            {
                entity.HasIndex(e => e.ClientAcct, "x_client_acct")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.AcctStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AcctType).IsUnicode(false);

                entity.Property(e => e.AllowBackdating)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AuthAttyFees)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AuthCellCalls)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AuthCollFees)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AuthEmails)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AuthTextMsgs)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChargeFees)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChargeInterest)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ClientAcct).IsUnicode(false);

                entity.Property(e => e.ClientContract)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ConsumerContract)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DirectsOnly)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DoNotMailInvoice)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DontWork)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FileJudgments)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FileProbate)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FileProofOfClaim)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FirstLetter).IsUnicode(false);

                entity.Property(e => e.Grade).IsUnicode(false);

                entity.Property(e => e.HipaaOnFile)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NoLongerPlacing)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PlacementType).IsUnicode(false);

                entity.Property(e => e.RemissionInterval)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RemitFullPmt)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ReportToBureau)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ZeroCommission)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ClientMasterP>(entity =>
            {
                entity.HasIndex(e => e.ClientAcct, "x_client_acct")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Address1).IsUnicode(false);

                entity.Property(e => e.Address12).IsUnicode(false);

                entity.Property(e => e.Address2).IsUnicode(false);

                entity.Property(e => e.Address22).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.City2).IsUnicode(false);

                entity.Property(e => e.ClientAcct).IsUnicode(false);

                entity.Property(e => e.ClientDesc).IsUnicode(false);

                entity.Property(e => e.ClientName).IsUnicode(false);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.FaxAreaCode).IsUnicode(false);

                entity.Property(e => e.FaxNum).IsUnicode(false);

                entity.Property(e => e.InfoVerified)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrigCreditor).IsUnicode(false);

                entity.Property(e => e.PhoneAreaCode).IsUnicode(false);

                entity.Property(e => e.PhoneAreaCode2).IsUnicode(false);

                entity.Property(e => e.PhoneExt2).IsUnicode(false);

                entity.Property(e => e.PhoneNum).IsUnicode(false);

                entity.Property(e => e.PhoneNum2).IsUnicode(false);

                entity.Property(e => e.PhoneticName).IsUnicode(false);

                entity.Property(e => e.StateCode).IsUnicode(false);

                entity.Property(e => e.StateCode2).IsUnicode(false);

                entity.Property(e => e.TaxId).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.Property(e => e.Zip2).IsUnicode(false);
            });

            modelBuilder.Entity<DebtorAcctInfoP>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.AccountAlert)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AcctDesc).IsUnicode(false);

                entity.Property(e => e.AcctStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AcctType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ActivityCode).IsUnicode(false);

                entity.Property(e => e.AgencyNum).IsUnicode(false);

                entity.Property(e => e.BankAcctClosed)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BillAs)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ClientRating).IsUnicode(false);

                entity.Property(e => e.ContactByMail)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ContactByPhone)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Corporate)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CosignerFirstName).IsUnicode(false);

                entity.Property(e => e.CosignerLastName).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.DoNotChargeInterest)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.EmailApproved)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EmailOptIn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EmailOptOut)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FinClass)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.InsuranceType).IsUnicode(false);

                entity.Property(e => e.Legal)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MailReturn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MediaOnFile)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NsfCheckOnFile)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrigLenderName).IsUnicode(false);

                entity.Property(e => e.OutOfStatute)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RestrictPromo)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ServiceAddrSame)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifAllowed)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorMasterP>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Address1).IsUnicode(false);

                entity.Property(e => e.Address2).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MaritalStatus).IsUnicode(false);

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.OrigAddress1).IsUnicode(false);

                entity.Property(e => e.OrigAddress2).IsUnicode(false);

                entity.Property(e => e.OrigCity).IsUnicode(false);

                entity.Property(e => e.OrigFirstName).IsUnicode(false);

                entity.Property(e => e.OrigLastName).IsUnicode(false);

                entity.Property(e => e.OrigSsn).IsUnicode(false);

                entity.Property(e => e.OrigState).IsUnicode(false);

                entity.Property(e => e.OrigZipCode).IsUnicode(false);

                entity.Property(e => e.ResidenceStatus).IsUnicode(false);

                entity.Property(e => e.Sex)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Spanish)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Ssn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SsnVer)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StateCode).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);
            });

            modelBuilder.Entity<QueueMasterP>(entity =>
            {
                entity.HasIndex(e => e.Employee, "x_employee")
                    .IsClustered();

                entity.Property(e => e.DebtorAcct).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
