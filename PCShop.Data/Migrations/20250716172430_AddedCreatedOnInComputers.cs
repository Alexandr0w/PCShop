using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedOnInComputers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Computers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date and time when the computer was created");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 17, 24, 29, 321, DateTimeKind.Utc).AddTicks(9695));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 14, 17, 24, 29, 321, DateTimeKind.Utc).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 13, 17, 24, 29, 321, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 12, 17, 24, 29, 321, DateTimeKind.Utc).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 11, 17, 24, 29, 321, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 10, 17, 24, 29, 321, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 17, 24, 29, 323, DateTimeKind.Utc).AddTicks(4571));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 14, 17, 24, 29, 323, DateTimeKind.Utc).AddTicks(4583));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 13, 17, 24, 29, 323, DateTimeKind.Utc).AddTicks(4587));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 12, 17, 24, 29, 323, DateTimeKind.Utc).AddTicks(4591));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 11, 17, 24, 29, 323, DateTimeKind.Utc).AddTicks(4595));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 10, 17, 24, 29, 323, DateTimeKind.Utc).AddTicks(4601));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Computers");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 15, 16, 34, 46, 385, DateTimeKind.Utc).AddTicks(8176));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 14, 16, 34, 46, 385, DateTimeKind.Utc).AddTicks(8187));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 13, 16, 34, 46, 385, DateTimeKind.Utc).AddTicks(8191));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 12, 16, 34, 46, 385, DateTimeKind.Utc).AddTicks(8195));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 11, 16, 34, 46, 385, DateTimeKind.Utc).AddTicks(8199));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 10, 16, 34, 46, 385, DateTimeKind.Utc).AddTicks(8205));
        }
    }
}
