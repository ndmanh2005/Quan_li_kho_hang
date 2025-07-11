using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Models;

namespace QuanLyKhoHang.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Profession> Professions { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Tier> Tiers { get; set; }
        public DbSet<HistoryTier> HistoryTiers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<CustomerSource> CustomerSources { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<EnterpriseUser> EnterpriseUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}