using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    public partial class ExistingDataDbContext : DbContext
    {
        public ExistingDataDbContext()
        {
        }

        public ExistingDataDbContext(DbContextOptions<ExistingDataDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<ClientMaster> ClientMasters { get; set; }
        public virtual DbSet<ClientMasterD> ClientMasterDs { get; set; }
        public virtual DbSet<ClientMasterH> ClientMasterHs { get; set; }
        public virtual DbSet<ClientMasterL> ClientMasterLs { get; set; }
        public virtual DbSet<ClientMasterT> ClientMasterTs { get; set; }
        public virtual DbSet<ClientMasterW> ClientMasterWs { get; set; }
        public virtual DbSet<DebtorAcctInfo> DebtorAcctInfos { get; set; }
        public virtual DbSet<DebtorAcctInfoD> DebtorAcctInfoDs { get; set; }
        public virtual DbSet<DebtorAcctInfoH> DebtorAcctInfoHs { get; set; }
        public virtual DbSet<DebtorAcctInfoL> DebtorAcctInfoLs { get; set; }
        public virtual DbSet<DebtorAcctInfoT> DebtorAcctInfoTs { get; set; }
        public virtual DbSet<DebtorAcctInfoW> DebtorAcctInfoWs { get; set; }
        public virtual DbSet<DebtorBadNumber> DebtorBadNumbers { get; set; }
        public virtual DbSet<DebtorPhoneInfo> DebtorPhoneInfos { get; set; }
        public virtual DbSet<NoteMaster> NoteMasters { get; set; }
        public virtual DbSet<CcPayment> CcPayments { get; set; }
        public virtual DbSet<DebtorMultiple> DebtorMultiples { get; set; }
        public virtual DbSet<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual DbSet<QueueMaster> QueueMasters { get; set; }
        public virtual DbSet<QueueMasterD> QueueMasterDs { get; set; }
        public virtual DbSet<QueueMasterH> QueueMasterHs { get; set; }
        public virtual DbSet<QueueMasterL> QueueMasterLs { get; set; }
        public virtual DbSet<QueueMasterT> QueueMasterTs { get; set; }
        public virtual DbSet<QueueMasterW> QueueMasterWs { get; set; }
        public virtual DbSet<MoveAccountApiLogs> MoveAccountApiLogs { get; set; }
        public virtual DbSet<NewPhoneNumber> NewPhoneNumbers { get; set; }
        public virtual DbSet<ApiMoveSetting> ApiMoveSettings { get; set; }
        public virtual DbSet<ApiMoveLog> ApiMoveLogs { get; set; }
        public virtual DbSet<EmployeeTimeLog> EmployeeTimeLogs { get; set; }
        public virtual DbSet<InteractResult> InteractResults { get; set; }
        public virtual DbSet<ClientAcctInfo> ClientAcctInfos { get; set; }
        public virtual DbSet<ClientAcctInfoD> ClientAcctInfoDs { get; set; }
        public virtual DbSet<ClientAcctInfoH> ClientAcctInfoHs { get; set; }
        public virtual DbSet<ClientAcctInfoL> ClientAcctInfoLs { get; set; }
        public virtual DbSet<ClientAcctInfoT> ClientAcctInfoTs { get; set; }
        public virtual DbSet<ClientAcctInfoW> ClientAcctInfoWs { get; set; }
        public virtual DbSet<DebtorMaster> DebtorMasters { get; set; }
        public virtual DbSet<DebtorMasterD> DebtorMasterDs { get; set; }
        public virtual DbSet<DebtorMasterH> DebtorMasterHs { get; set; }
        public virtual DbSet<DebtorMasterL> DebtorMasterLs { get; set; }
        public virtual DbSet<DebtorMasterT> DebtorMasterTs { get; set; }
        public virtual DbSet<DebtorMasterW> DebtorMasterWs { get; set; }
        public virtual DbSet<CheckDetail> CheckDetails { get; set; }
        public virtual DbSet<DebtorPpInfo> DebtorPpInfos { get; set; }
        public virtual DbSet<LarryCcPayment> LarryCcPayments { get; set; }
        public virtual DbSet<LcgCardInfo> LcgCardInfos { get; set; }
        public virtual DbSet<LcgPaymentSchedule> LcgPaymentSchedules { get; set; }
        public virtual DbSet<LcgPaymentScheduleHistory> LcgPaymentScheduleHistories { get; set; }
        public virtual DbSet<LarryCcIndex> LarryCcIndices { get; set; }
        public virtual DbSet<LarryCcIndex2> LarryCcIndex2s { get; set; }
        public virtual DbSet<PatientMaster> PatientMasters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=10.56.71.2;Database=collect;User Id=stephen;Password=Arizona2020!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ClientMaster>(entity =>
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

            modelBuilder.Entity<ClientMasterD>(entity =>
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

            modelBuilder.Entity<ClientMasterH>(entity =>
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

            modelBuilder.Entity<ClientMasterL>(entity =>
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

            modelBuilder.Entity<ClientMasterT>(entity =>
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

            modelBuilder.Entity<ClientMasterW>(entity =>
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

            modelBuilder.Entity<DebtorAcctInfo>(entity =>
            {
                entity.HasIndex(e => e.ActivityCode, "x_activity_code")
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered()
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => new { e.Employee, e.ScheduleDate }, "x_emp_sched_date")
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => e.SuppliedAcct, "x_supplied_acct")
                    .HasFillFactor((byte)70);

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

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorAcctInfoD>(entity =>
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

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorAcctInfoH>(entity =>
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

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorAcctInfoL>(entity =>
            {
                entity.HasIndex(e => e.ActivityCode, "x_activity_code")
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered()
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => new { e.Employee, e.ScheduleDate }, "x_emp_sched_date")
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => e.SuppliedAcct, "x_supplied_acct")
                    .HasFillFactor((byte)70);

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

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorAcctInfoT>(entity =>
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

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DebtorAcctInfoW>(entity =>
            {
                entity.HasIndex(e => e.ActivityCode, "x_activity_code")
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered()
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => new { e.Employee, e.ScheduleDate }, "x_emp_sched_date")
                    .HasFillFactor((byte)70);

                entity.HasIndex(e => e.SuppliedAcct, "x_supplied_acct")
                    .HasFillFactor((byte)70);

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

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
