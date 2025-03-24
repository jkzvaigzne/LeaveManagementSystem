using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLeaveRequestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "LeaveRequest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "LeaveRequest",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "LeaveRequestStatusId",
                table: "LeaveRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "LeaveRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequestComments",
                table: "LeaveRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerId",
                table: "LeaveRequest",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "LeaveRequest",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1d759058-7ba0-4e62-9250-3dfa5844f9f2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eb7aeb59-fcd3-46cd-95d5-157dc4e2e9cf", "AQAAAAIAAYagAAAAEFqIK8ZK4By+F1aT4QKEUz/JIW/MiMUbCdz7YwaBGnx+F35kw9a/95ZsxR2w6Wv4Dw==", "5afe06b0-466d-444b-a793-bd42f97b9e3c" });

            migrationBuilder.InsertData(
                table: "LeaveRequestStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Declined" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeId",
                table: "LeaveRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_LeaveRequestStatusId",
                table: "LeaveRequest",
                column: "LeaveRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_LeaveTypeId",
                table: "LeaveRequest",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_ReviewerId",
                table: "LeaveRequest",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequest_AspNetUsers_EmployeeId",
                table: "LeaveRequest",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequest_AspNetUsers_ReviewerId",
                table: "LeaveRequest",
                column: "ReviewerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequest_LeaveRequestStatuses_LeaveRequestStatusId",
                table: "LeaveRequest",
                column: "LeaveRequestStatusId",
                principalTable: "LeaveRequestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequest_LeaveTypes_LeaveTypeId",
                table: "LeaveRequest",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequest_AspNetUsers_EmployeeId",
                table: "LeaveRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequest_AspNetUsers_ReviewerId",
                table: "LeaveRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequest_LeaveRequestStatuses_LeaveRequestStatusId",
                table: "LeaveRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequest_LeaveTypes_LeaveTypeId",
                table: "LeaveRequest");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequest_EmployeeId",
                table: "LeaveRequest");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequest_LeaveRequestStatusId",
                table: "LeaveRequest");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequest_LeaveTypeId",
                table: "LeaveRequest");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequest_ReviewerId",
                table: "LeaveRequest");

            migrationBuilder.DeleteData(
                table: "LeaveRequestStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveRequestStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveRequestStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveRequestStatuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "LeaveRequest");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "LeaveRequest");

            migrationBuilder.DropColumn(
                name: "LeaveRequestStatusId",
                table: "LeaveRequest");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "LeaveRequest");

            migrationBuilder.DropColumn(
                name: "RequestComments",
                table: "LeaveRequest");

            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "LeaveRequest");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "LeaveRequest");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1d759058-7ba0-4e62-9250-3dfa5844f9f2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0034fa2-61a4-4e66-a787-d25fb3975462", "AQAAAAIAAYagAAAAEPGxR+JKSMY8wgd60sGbhv+GUiIzVvCx+wGbvwy5Eeq0mJitHNXLhTOCFU6M6p0Lcw==", "8bd4291f-825d-4f94-9cd9-3e2a57069701" });
        }
    }
}
