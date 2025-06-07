using System;
using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Data.Entities.Tasks;
using EMS.Data.Entities.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EMS.Data
{
    public class EmpDbContext : IdentityDbContext<IdentityUser>
    {
        public EmpDbContext(DbContextOptions<EmpDbContext> options)
         : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EmpSalary> EmpSalaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                   .Build();
                var connectionString = configuration.GetConnectionString("default");
                optionsBuilder.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("EMS"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            seedUserData(modelBuilder);
        }

        private static void seedUserData(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin", Id = "99ec3a4e-66d4-4442-a703-794ae8b72856" },
                new IdentityRole() { Name = "Superadmin", ConcurrencyStamp = "2", NormalizedName = "Superdmin", Id = "ea1ad442-0ee1-4ebc-b1a7-edcbd7098f9e" },
                new IdentityRole() { Name = "Employee", ConcurrencyStamp = "3", NormalizedName = "Employee", Id= "f0f7cff8-9762-484f-8e30-b918b711aff9" }
                );
            var hasher = new PasswordHasher<Employee>();
            builder.Entity<Employee>().HasData
               (
               new Employee()
               {
                   Id = "224047d7-9c97-4768-a0f6-50e7eec9fe29",
                   UserName = "admin@123.com",
                   Email = "admin@123.com",
                   NormalizedUserName = "ADMIN@123.COM",
                   NormalizedEmail = "ADMIN@123.COM",
                   AadharNumber= "85697 13358 854",
                   EmailConfirmed = true,
                   PasswordHash = hasher.HashPassword(null, "Test@123"),
                   Firstname = "hardikadmin",
                   Lastname = "masalawala",
                   Address = "surat",
               },
                new Employee()
                {
                    Id = "d8545447-30d8-4910-8931-b5c22e47f8ce",
                    UserName = "superadmin@123.com",
                    Email = "superadmin@123.com",
                    NormalizedUserName = "SUPERADMIN@123.COM",
                    NormalizedEmail= "SUPERADMIN@123.COM",
                    AadharNumber= "25214 9652 452",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Test@123"),
                    Firstname = "hardikSadmin",
                    Lastname = "masalawala",
                    Address = "surat",
                },
                new Employee()
                {
                    Id = "3f72a68b-1d4e-4c7b-9f0a-5e3d8c2e6b59",
                    UserName = "employee@123.com",
                    Email = "employee@123.com",
                    NormalizedUserName = "EMPLOYEE@123.COM",
                    NormalizedEmail = "EMPLOYEE@123.COM",
                    AadharNumber = "25214 9652 452",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Test@123"),
                    Firstname = "hardikemployee",
                    Lastname = "masalawala",
                    Address = "surat",
                }
               ); 

            List<Employee> users = new List<Employee>();

            List<IdentityRole> roles = new List<IdentityRole>();
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = "224047d7-9c97-4768-a0f6-50e7eec9fe29", RoleId = "99ec3a4e-66d4-4442-a703-794ae8b72856" },
                new IdentityUserRole<string>() { UserId = "d8545447-30d8-4910-8931-b5c22e47f8ce", RoleId = "ea1ad442-0ee1-4ebc-b1a7-edcbd7098f9e" },
                new IdentityUserRole<string>() { UserId = "3f72a68b-1d4e-4c7b-9f0a-5e3d8c2e6b59", RoleId = "f0f7cff8-9762-484f-8e30-b918b711aff9" }
             );
        }
    }
}