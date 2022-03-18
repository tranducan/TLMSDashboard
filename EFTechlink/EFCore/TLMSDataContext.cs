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

        public virtual DbSet<MErpmqcRealtime> MErpmqcRealtimes { get; set; }

        public virtual DbSet<PQCMesData> PqcmesData { get; set; }

        public virtual DbSet<DailyPerformanceGoal> DailyPerformanceGoals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-M6N0IBR\\SQLEXPRESS;Initial Catalog=ERPSOFT;Integrated Security=SSPI;Application Name=DBGenerationCommand");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<MErpmqcRealtime>(entity =>
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

            modelBuilder.Entity<DailyPerformanceGoal>(entity =>
            {
                entity.HasKey(e => e.DailyPerformanceGoaId)
                    .HasName("PK_DailyPerformanceGoal_DailyPerformanceGoalid");

                entity.ToTable("DailyPerformanceGoal", "ProcessHistory");

                entity.Property(e => e.Line).HasMaxLength(20);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NotGoodTarget).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.OutputTarget).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.Process)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ReworkTarget).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.Site).HasMaxLength(20);

                entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");

            });

            modelBuilder.Entity<PQCMesData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PQCMesData", "ProcessHistory");

                entity.Property(e => e.Attribute)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.AttributeType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Flag).HasMaxLength(10);

                entity.Property(e => e.InspectDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Inspector)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastModifiedUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(suser_sname())");

                entity.Property(e => e.LastTimeModified).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Line)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LotNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pocode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("POCode");

                entity.Property(e => e.PqcmesDataId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PQCMesDataId");

                entity.Property(e => e.Process)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Quantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.VersionNumber)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
