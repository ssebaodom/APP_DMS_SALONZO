using Microsoft.EntityFrameworkCore;
using SSE.Common.Entities.v1;

namespace SSE.DataAccess.Context
{
    public partial class GlobalDbContext : DbContext
    {
        public GlobalDbContext()
        {
        }

        public GlobalDbContext(DbContextOptions<GlobalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbSysInfo> DbSysInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbSysInfo>(entity =>
            {
                entity.HasKey(e => new { e.HostId, e.UserName })
                    .HasName("PK_entitydv_1");

                entity.Property(e => e.HostId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SqlPassLogin)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SqlUserLogin)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SysDbName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}