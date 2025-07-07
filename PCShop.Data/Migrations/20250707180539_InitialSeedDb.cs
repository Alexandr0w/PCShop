using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "123 Admin Street", "Admin City", "ff2600d1-23f5-4b4e-bab7-be279a518cd8", "ApplicationUser", "admin@pcshop.com", true, "Admin User", false, null, "ADMIN@PCSHOP.COM", "ADMIN@PCSHOP.COM", "AQAAAAIAAYagAAAAEMra+qYypRsAiWX9n92ghpucxtO6D4qnCxESFgcqJabfnUuhciz0bSvLYZoBfnlP3A==", null, false, "00000", "1e2044db-7e3a-43d7-b91d-b721c8dbdb65", false, "admin@pcshop.com" });

            migrationBuilder.InsertData(
                table: "Computers",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"), "CPU: AMD Ryzen 5 5600, GPU: GeForce RTX 3060TI, RAM: 32GB DDR4, Storage: 1TB SSD NVMe...", "images/computers/grigs_polaris_max_amd.png", "G:RIGS POLARIS Max (AMD)", 2000.00m },
                    { new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"), "CPU: Intel Core i5-13400F, GPU: GeForce RTX 3060TI, RAM: 32GB DDR4, Storage: 1TB SSD NVMe...", "images/computers/grigs_polaris_max_intel.png", "G:RIGS POLARIS Max (Intel)", 2100.00m },
                    { new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"), "CPU: AMD Ryzen 7 5700X3D, GPU: GeForce RTX 4060, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...", "images/computers/grigs_spark_ultra_amd_x3d.png", "G:RIGS SPARK Ultra (AMD X3D)", 2500.00m },
                    { new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"), "CPU: Intel Core i5-14600KF, GPU: GeForce RTX 5070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...", "images/computers/grigs_nova_ultra_intel.png", "G:RIGS NOVA Ultra (Intel)", 3690.00m },
                    { new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"), "CPU: AMD Ryzen 7 7700, GPU: GeForce RTX 5070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...", "images/computers/grigs_sirius_ultra_amd_zen4_wh.png", "G:RIGS SIRIUS Ultra (AMD Zen4)", 4200.00m },
                    { new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"), "CPU: Intel Core i7-13700K, GPU: GeForce RTX 4070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...", "images/computers/grigs_sirius_ultra_intel.png", "G:RIGS SIRIUS Ultra (Intel)", 4500.00m }
                });

            migrationBuilder.InsertData(
                table: "ProductsTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-bbbb-cccc-111111111111"), "Processor" },
                    { new Guid("22222222-aaaa-bbbb-cccc-222222222222"), "Video Card" },
                    { new Guid("33333333-aaaa-bbbb-cccc-333333333333"), "Motherboard" },
                    { new Guid("44444444-aaaa-bbbb-cccc-444444444444"), "RAM" },
                    { new Guid("55555555-aaaa-bbbb-cccc-555555555555"), "SSD" },
                    { new Guid("66666666-aaaa-bbbb-cccc-666666666666"), "HDD" },
                    { new Guid("77777777-aaaa-bbbb-cccc-777777777777"), "Power Supply" },
                    { new Guid("88888888-aaaa-bbbb-cccc-888888888888"), "Cooling System" },
                    { new Guid("99999999-aaaa-bbbb-cccc-999999999999"), "Case Fan" },
                    { new Guid("aaaaaaa1-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Case" },
                    { new Guid("aaaaaaa2-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Monitor" },
                    { new Guid("aaaaaaa3-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Keyboard" },
                    { new Guid("aaaaaaa4-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Mouse" },
                    { new Guid("aaaaaaa5-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Headset" },
                    { new Guid("aaaaaaa6-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Mousepad" },
                    { new Guid("aaaaaaa7-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Speakers" },
                    { new Guid("aaaaaaa8-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Microphone" },
                    { new Guid("aaaaaaa9-aaaa-bbbb-cccc-aaaaaaaaaaaa"), "Webcam" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price", "ProductTypeId" },
                values: new object[,]
                {
                    { new Guid("10101010-aaaa-bbbb-cccc-101010101010"), "13th Gen 16-core processor with high performance for gaming and productivity.", "/images/products/intel-i7-13700k.jpg", "Intel Core i7-13700K", 489.99m, new Guid("11111111-aaaa-bbbb-cccc-111111111111") },
                    { new Guid("20202020-aaaa-bbbb-cccc-202020202020"), "Latest generation graphics card with ray tracing and DLSS support.", "/images/products/rtx-4070.jpg", "NVIDIA GeForce RTX 4070", 629.00m, new Guid("22222222-aaaa-bbbb-cccc-222222222222") },
                    { new Guid("30303030-aaaa-bbbb-cccc-303030303030"), "High-performance DDR4 memory kit with RGB lighting.", "/images/products/corsair-vengeance-32gb.jpg", "Corsair Vengeance RGB Pro 32GB DDR4", 139.99m, new Guid("44444444-aaaa-bbbb-cccc-444444444444") },
                    { new Guid("40404040-aaaa-bbbb-cccc-404040404040"), "Ultra-fast PCIe 4.0 SSD ideal for gaming and heavy applications.", "/images/products/samsung-980-pro.jpg", "Samsung 980 PRO 1TB NVMe SSD", 119.49m, new Guid("55555555-aaaa-bbbb-cccc-555555555555") },
                    { new Guid("50505050-aaaa-bbbb-cccc-505050505050"), "Premium Z790 chipset motherboard supporting 13th Gen Intel CPUs.", "/images/products/asus-z790.jpg", "ASUS ROG STRIX Z790-E", 379.00m, new Guid("33333333-aaaa-bbbb-cccc-333333333333") },
                    { new Guid("60606060-aaaa-bbbb-cccc-606060606060"), "Reliable and efficient 750W power supply unit with modular cables.", "/images/products/cooler-master-750w.jpg", "Cooler Master 750W 80+ Gold PSU", 109.99m, new Guid("77777777-aaaa-bbbb-cccc-777777777777") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd");

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10101010-aaaa-bbbb-cccc-101010101010"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20202020-aaaa-bbbb-cccc-202020202020"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30303030-aaaa-bbbb-cccc-303030303030"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40404040-aaaa-bbbb-cccc-404040404040"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50505050-aaaa-bbbb-cccc-505050505050"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("60606060-aaaa-bbbb-cccc-606060606060"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("66666666-aaaa-bbbb-cccc-666666666666"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("88888888-aaaa-bbbb-cccc-888888888888"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("99999999-aaaa-bbbb-cccc-999999999999"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa4-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa5-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa6-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa7-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa8-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa9-aaaa-bbbb-cccc-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-bbbb-cccc-111111111111"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-aaaa-bbbb-cccc-222222222222"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-aaaa-bbbb-cccc-333333333333"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-aaaa-bbbb-cccc-444444444444"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("55555555-aaaa-bbbb-cccc-555555555555"));

            migrationBuilder.DeleteData(
                table: "ProductsTypes",
                keyColumn: "Id",
                keyValue: new Guid("77777777-aaaa-bbbb-cccc-777777777777"));
        }
    }
}
