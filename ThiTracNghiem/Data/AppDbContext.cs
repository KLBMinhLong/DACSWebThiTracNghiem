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

        public DbSet<LichSuLamBai> LichSuLamBais { get; set; }
        public DbSet<ChiTietLamBai> ChiTietLamBais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietLamBai>()
                .HasOne(c => c.LichSuLamBai)
                .WithMany(l => l.ChiTietLamBais)
                .HasForeignKey(c => c.LichSuLamBaiId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<BinhLuan> BinhLuans { get; set; }
        public DbSet<LienHe> LienHes { get; set; }

    }
}
