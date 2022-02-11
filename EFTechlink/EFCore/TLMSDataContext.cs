using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EFTechlink.EFCore
{
    public partial class TLMSDataContext : DbContext
    {
        public TLMSDataContext()
        {
        }

        public TLMSDataContext(DbContextOptions<TLMSDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MQCRealtime> MErpmqcRealtimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=FS-35686\\SQLEXPRESS;Initial Catalog=ERPSOFT;Integrated Security=SSPI;Application Name=DBGenerationCommand");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<MQCRealtime>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("m_ERPMQC_REALTIME");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("data");

                entity.Property(e => e.Factory)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("factory");

                entity.Property(e => e.Inspectdate)
                    .HasColumnType("date")
                    .HasColumnName("inspectdate");

                entity.Property(e => e.Inspecttime).HasColumnName("inspecttime");

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("item");

                entity.Property(e => e.Judge)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("judge");

                entity.Property(e => e.Line)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("line");

                entity.Property(e => e.Lot)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("lot");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("model");

                entity.Property(e => e.Process)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("process");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("remark");

                entity.Property(e => e.Serno)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("serno");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("site");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
