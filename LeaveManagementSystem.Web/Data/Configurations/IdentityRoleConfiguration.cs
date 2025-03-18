using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
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
       }
    }
}
