using System.Reflection;

namespace LeaveManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //builder.ApplyConfiguration(new ApplicationUserConfiguration());
        //builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
        //builder.ApplyConfiguration(new IdentityRoleConfiguration());
        //builder.ApplyConfiguration(new LeaveRequestStatusConfiguration());
    }

    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocation { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }
    public DbSet<LeaveRequest> LeaveRequest { get; set; }
}
