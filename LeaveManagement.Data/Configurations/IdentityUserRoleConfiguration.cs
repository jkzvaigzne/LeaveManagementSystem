namespace LeaveManagement.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "df494301-35c8-49bd-8d17-08ddb9fb8f3a",
                UserId = "1d759058-7ba0-4e62-9250-3dfa5844f9f2"
            });
        }
    }
}
