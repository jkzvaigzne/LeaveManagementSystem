using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "ac8bf66d-d207-405e-a09d-d088a676a784",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            },
            new IdentityRole
            {
                Id = "826a51a9-020d-4b9c-94b8-b466dddb1340",
                Name = "Supervisor",
                NormalizedName = "SUPERVISOR"
            },
            new IdentityRole
            {
                Id = "df494301-35c8-49bd-8d17-08ddb9fb8f3a",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        );

        var hasher = new PasswordHasher<ApplicationUser>();
        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = "1d759058-7ba0-4e62-9250-3dfa5844f9f2",
            Email = "admin@localhost.com",
            NormalizedEmail = "ADMIN@LOCALHOST.COM",
            UserName = "admin@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "Admin",
            DateOfBirth = new DateOnly(1995, 05, 10)
        });

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "df494301-35c8-49bd-8d17-08ddb9fb8f3a",
                UserId = "1d759058-7ba0-4e62-9250-3dfa5844f9f2"
            });
        }

        public DbSet<LeaveType> LeaveTypes{ get; set; }
        public DbSet<LeaveAllocation> LeaveAllocation { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }
        public DbSet<LeaveRequest> LeaveRequest { get; set; }
}
