using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedOnToProductsAndComputers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Products",
                type: "datetime2",
                nullable: true,
                comment: "Date and time when the product was deleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Computers",
                type: "datetime2",
                nullable: true,
                comment: "Date and time when the computer was deleted");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 20, 13, 19, 40, 824, DateTimeKind.Utc).AddTicks(2716), null });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 19, 13, 19, 40, 824, DateTimeKind.Utc).AddTicks(2732), null });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 18, 13, 19, 40, 824, DateTimeKind.Utc).AddTicks(2737), null });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 19, 40, 824, DateTimeKind.Utc).AddTicks(2742), null });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 19, 40, 824, DateTimeKind.Utc).AddTicks(2833), null });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 15, 13, 19, 40, 824, DateTimeKind.Utc).AddTicks(2843), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 20, 13, 19, 40, 826, DateTimeKind.Utc).AddTicks(766), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 19, 13, 19, 40, 826, DateTimeKind.Utc).AddTicks(777), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 18, 13, 19, 40, 826, DateTimeKind.Utc).AddTicks(781), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 19, 40, 826, DateTimeKind.Utc).AddTicks(785), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 19, 40, 826, DateTimeKind.Utc).AddTicks(788), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                columns: new[] { "CreatedOn", "DeletedOn" },
                values: new object[] { new DateTime(2025, 7, 15, 13, 19, 40, 826, DateTimeKind.Utc).AddTicks(793), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Computers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), 0, "123 Admin Street", "Admin City", "20156ffd-ca5b-493f-8cdb-f0e37d6ffc28", "admin@pcshop.com", true, "Admin User", false, null, "ADMIN@PCSHOP.COM", "ADMIN@PCSHOP.COM", "AQAAAAIAAYagAAAAEN6HKPt1nIvfpyU80C1hHBdt/MBp8t8qhTJqbc55PHUBGvA9g2Y47ybCLa5zyIrOPg==", null, false, "0000", "cdd1077d-f80c-4f64-b3bc-e7b71fc4ef9a", false, "admin@pcshop.com" });

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 18, 58, 36, 45, DateTimeKind.Utc).AddTicks(4506));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 14, 18, 58, 36, 45, DateTimeKind.Utc).AddTicks(4519));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 13, 18, 58, 36, 45, DateTimeKind.Utc).AddTicks(4523));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 12, 18, 58, 36, 45, DateTimeKind.Utc).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 11, 18, 58, 36, 45, DateTimeKind.Utc).AddTicks(4539));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 10, 18, 58, 36, 45, DateTimeKind.Utc).AddTicks(4544));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 18, 58, 36, 47, DateTimeKind.Utc).AddTicks(819));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 14, 18, 58, 36, 47, DateTimeKind.Utc).AddTicks(828));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 13, 18, 58, 36, 47, DateTimeKind.Utc).AddTicks(832));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 12, 18, 58, 36, 47, DateTimeKind.Utc).AddTicks(835));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 11, 18, 58, 36, 47, DateTimeKind.Utc).AddTicks(838));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 10, 18, 58, 36, 47, DateTimeKind.Utc).AddTicks(847));
        }
    }
}
