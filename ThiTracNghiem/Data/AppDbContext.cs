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

    }
}
