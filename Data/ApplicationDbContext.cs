using BTL.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<NhanVien> NhanVien { get ; set; }
        public DbSet<NhaCungCap> NhaCungCap { get ; set; }
        public DbSet<BTL.Models.NhomThuoc> NhomThuoc { get; set; } = default!;
        public DbSet<BTL.Models.Thuoc> Thuoc { get; set; } = default!;
    }
}