using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "826a51a9-020d-4b9c-94b8-b466dddb1340", null, "Supervisor", "SUPERVISOR" },
                    { "ac8bf66d-d207-405e-a09d-d088a676a784", null, "Employee", "EMPLOYEE" },
                    { "df494301-35c8-49bd-8d17-08ddb9fb8f3a", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1d759058-7ba0-4e62-9250-3dfa5844f9f2", 0, "cf6a1437-f7ce-4c7e-bc15-81c8e69d10f5", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", null, "AQAAAAIAAYagAAAAEPHuQ79qNv4BFR5QqF8a2yHwXEt1IMRMWjUmtzR85NsRloTnHa3jYH5t/jNqEsg21A==", null, false, "2cd132ae-fcb1-4b8a-89c8-11260875a060", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "df494301-35c8-49bd-8d17-08ddb9fb8f3a", "1d759058-7ba0-4e62-9250-3dfa5844f9f2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "826a51a9-020d-4b9c-94b8-b466dddb1340");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac8bf66d-d207-405e-a09d-d088a676a784");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "df494301-35c8-49bd-8d17-08ddb9fb8f3a", "1d759058-7ba0-4e62-9250-3dfa5844f9f2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df494301-35c8-49bd-8d17-08ddb9fb8f3a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1d759058-7ba0-4e62-9250-3dfa5844f9f2");
        }
    }
}
