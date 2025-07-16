using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedOnInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date and time when the product was created");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Products");
        }
    }
}
