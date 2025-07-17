using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryInfoToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Additional customer comment for the order");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                comment: "Final delivery address composed of city, postal code, and address");

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Total delivery fee based on delivery method");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMethod",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Delivery method for the order");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                table: "Orders");

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
    }
}
