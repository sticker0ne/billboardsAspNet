using System.Data.Entity;

namespace BillboardsMVC5.Models
{
    public class BillboardContext : DbContext
    {
        public BillboardContext() : base("name=BillboardsCs")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(u => new { u.UserId, u.RoleId });
        }
        public DbSet<Billboard> Billboards { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}