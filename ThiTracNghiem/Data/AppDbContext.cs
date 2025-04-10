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
    }
}
