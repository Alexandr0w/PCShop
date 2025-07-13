using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class PricesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "Price",
                value: 1022.11m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "Price",
                value: 1109.04m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "Price",
                value: 1241.98m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "Price",
                value: 1876.01m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "Price",
                value: 2039.63m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "Price",
                value: 1870.89m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "Price",
                value: 255.66m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "Price",
                value: 766.00m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "Price",
                value: 178.45m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "Price",
                value: 101.75m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "Price",
                value: 204.53m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "Price",
                value: 108.84m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "Price",
                value: 2000.00m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "Price",
                value: 2100.00m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "Price",
                value: 2500.00m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "Price",
                value: 3690.00m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "Price",
                value: 4200.00m);

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "Price",
                value: 4500.00m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"),
                column: "Price",
                value: 489.99m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "Price",
                value: 629.00m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "Price",
                value: 139.99m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "Price",
                value: 119.49m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "Price",
                value: 379.00m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "Price",
                value: 109.99m);
        }
    }
}
