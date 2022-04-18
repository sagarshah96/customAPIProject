using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.ApplicationContext
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Login> Login { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(new Roles { RoleId = 1, RoleName = "Admin" }, new Roles { RoleId = 2, RoleName = "Customer" });
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerId = 1, FirstName = "Admin", LastName = "Shah", MiddleName = "K", MobileNo = "7385485001", IsActive = true });
            modelBuilder.Entity<Login>().HasData(new Login { LoginId = 1, Email = "admin@gmail.com", Password = "admin@123", CustomerId = 1, RoleId = 1 });
        }
    }
}
