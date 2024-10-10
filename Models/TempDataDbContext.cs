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

        public virtual DbSet<AiCallResult> AiCallResults { get; set; }

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

            modelBuilder.Entity<AiCallResult>(entity =>
            {
                entity.Property(e => e.CallDisposition).IsUnicode(false);

                entity.Property(e => e.CallPhoneNumber).IsUnicode(false);

                entity.Property(e => e.CallStatus).IsUnicode(false);

                entity.Property(e => e.CallType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
