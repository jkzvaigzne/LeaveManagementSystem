namespace LeaveManagement.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(new ApplicationUser
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
        }
    }
}
