using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e641713-c0ea-4469-b5dc-307f9d80ed9b", "AQAAAAIAAYagAAAAEK4NtstxllOph/nO7oXfcRvk7KAsVSwua7kiK9A+zDScHi+MuvZYpqUxYySovnWEaA==", "ce539fe8-e2ab-4728-a60b-f0eac43001ba" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "ImageUrl",
                value: "/images/products/rtx-4070.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "ImageUrl",
                value: "/images/products/corsair-vengeance-32gb.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "ImageUrl",
                value: "/images/products/samsung-980-pro.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "ImageUrl",
                value: "/images/products/asus-z790.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "ImageUrl",
                value: "/images/products/cooler-master-750w.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d021a7e6-ab5b-452c-8f0e-9aa363c19e48", "AQAAAAIAAYagAAAAELeYEnzn9cElK/GbOVEO+vjZ7jb2uhHl9gepiCJBCziS0TV4gMIxPvOPOmsPMkuxXA==", "d9568473-201c-4a30-89b0-27b90726a580" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"),
                column: "ImageUrl",
                value: "/images/products/rtx-4070.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"),
                column: "ImageUrl",
                value: "/images/products/corsair-vengeance-32gb.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"),
                column: "ImageUrl",
                value: "/images/products/samsung-980-pro.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"),
                column: "ImageUrl",
                value: "/images/products/asus-z790.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"),
                column: "ImageUrl",
                value: "/images/products/cooler-master-750w.jpg");
        }
    }
}
