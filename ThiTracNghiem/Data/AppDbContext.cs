using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Models;

namespace ThiTracNghiem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<ChuDe> ChuDes { get; set; }
        public DbSet<CauHoi> CauHois { get; set; }
        public DbSet<DeThi> DeThis { get; set; }
        public DbSet<ChiTietDeThi> ChiTietDeThis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChiTietDeThi>()
                .HasOne(ct => ct.DeThi)
                .WithMany(d => d.ChiTietCauHoi)
                .HasForeignKey(ct => ct.DeThiId)
                .OnDelete(DeleteBehavior.Restrict); // tránh multiple cascade

            modelBuilder.Entity<ChiTietDeThi>()
                .HasOne(ct => ct.CauHoi)
                .WithMany()
                .HasForeignKey(ct => ct.CauHoiId)
                .OnDelete(DeleteBehavior.Cascade); // vẫn cho phép xóa câu hỏi kéo theo
        }

    }
}
