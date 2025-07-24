using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the notification"),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign key to the ApplicationUser receiving the notification"),
                    Message = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "The title of the notification"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "The date and time when the notification was created"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates whether the notification has been read by the user")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 18, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 17, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 18, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 17, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 23, 7, 55, 3, 199, DateTimeKind.Utc).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 22, 7, 55, 3, 199, DateTimeKind.Utc).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 21, 7, 55, 3, 199, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 20, 7, 55, 3, 199, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 19, 7, 55, 3, 199, DateTimeKind.Utc).AddTicks(5038));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 18, 7, 55, 3, 199, DateTimeKind.Utc).AddTicks(5043));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 23, 7, 55, 3, 201, DateTimeKind.Utc).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 22, 7, 55, 3, 201, DateTimeKind.Utc).AddTicks(1921));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 21, 7, 55, 3, 201, DateTimeKind.Utc).AddTicks(1924));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 20, 7, 55, 3, 201, DateTimeKind.Utc).AddTicks(1927));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 19, 7, 55, 3, 201, DateTimeKind.Utc).AddTicks(1935));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 18, 7, 55, 3, 201, DateTimeKind.Utc).AddTicks(1939));
        }
    }
}
