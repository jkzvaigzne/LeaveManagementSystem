using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1d759058-7ba0-4e62-9250-3dfa5844f9f2",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dc6496f7-341e-4a2a-a141-d8fb2ffbe952", new DateOnly(1995, 5, 10), "Default", "Admin", "AQAAAAIAAYagAAAAEDJEFtglkLrOaoe/pYYXwim+sGJfLXPK0wBVI78/Em5LwJ1KUHawvwwZKC+9lwik2w==", "ded074ee-9c11-4ca4-aa5d-0185fd0757d6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1d759058-7ba0-4e62-9250-3dfa5844f9f2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf6a1437-f7ce-4c7e-bc15-81c8e69d10f5", "AQAAAAIAAYagAAAAEPHuQ79qNv4BFR5QqF8a2yHwXEt1IMRMWjUmtzR85NsRloTnHa3jYH5t/jNqEsg21A==", "2cd132ae-fcb1-4b8a-89c8-11260875a060" });
        }
    }
}
