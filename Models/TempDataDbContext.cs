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

            modelBuilder.Entity<InteractResult>(entity =>
            {
                entity.Property(e => e.Ani).IsUnicode(false);

                entity.Property(e => e.CallResult).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.LastDialogue).IsUnicode(false);

                entity.Property(e => e.OpeningIntent).IsUnicode(false);

                entity.Property(e => e.TermReason).IsUnicode(false);

                entity.Property(e => e.TransferReason).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
