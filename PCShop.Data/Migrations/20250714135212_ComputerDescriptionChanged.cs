using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class ComputerDescriptionChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "Description",
                value: "CPU: AMD Ryzen 5 5600 // GPU: MSI GeForce RTX 3060 VENTUS 2X 12G // RAM: 32GB (2x 16GB) DDR4 3600 MT/s // Storage: 1TB Kingston NV3 // Motherboard: ASRock B550M Pro4 // Case: DeepCool CH360 DIGITAL // Cooling: DeepCool AG400 Black ARGB // Power Supply: DeepCool PK650D 650W Bronze");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "Description",
                value: "CPU: Intel Core i5-13400F // GPU: MSI GeForce RTX 3060 VENTUS 2X 12G // RAM: 32GB (2x 16GB) DDR4 3600 MT/s // Storage: 1TB Kingston NV3 // Motherboard: ASUS TUF GAMING B760M-PLUS D4 // Case: DeepCool CH360 DIGITAL WH // Cooling: DeepCool AG400 White ARGB // Power Supply: DeepCool PK650D 650W Bronze");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "Description",
                value: "CPU: AMD Ryzen 7 5700X3D // GPU: MSI GeForce RTX 4060 VENTUS 2X BLACK // RAM: 32GB (2x16GB) DDR4 3200 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B550-A PRO // Case: COUGAR Duoface RGB // Cooling: DeepCool AG400 Black ARGB // Power Supply: DeepCool PK750D 750W Bronze");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "Description",
                value: "CPU: Intel Core i5-14600KF // GPU: MSI GeForce RTX 5070 12G SHADOW 2X // RAM: 32GB (2x16GB) DDR5 6000 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B760 GAMING PLUS WIFI // Case: 1stPlayer MEGAVIEW MV8 Black // Cooling: DeepCool AG620 BK ARGB // Power Supply: DeepCool PN750M ATX 3.1");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "Description",
                value: "CPU: AMD Ryzen 7 7700 // GPU: MSI GeForce RTX 5070 12G SHADOW 2X // RAM: 32GB (2x16GB) DDR5 6000 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B850 GAMING PLUS WIFI // Case: DeepCool CH560 DIGITAL Black // Cooling: DeepCool AG620 BK ARGB // Power Supply: DeepCool PN750M ATX 3.1");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "Description",
                value: "CPU: Intel Core i5-14600KF // GPU: MSI GeForce RTX 5070 12G SHADOW 2X // RAM: 32GB (2x16GB) DDR5 6000 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B760 GAMING PLUS WIFI // Case: DeepCool CH560 DIGITAL Black // Cooling: DeepCool AG620 BK ARGB // Power Supply: DeepCool PN750M ATX 3.1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f1f11111-aaaa-bbbb-cccc-111111111111"),
                column: "Description",
                value: "CPU: AMD Ryzen 5 5600, GPU: GeForce RTX 3060TI, RAM: 32GB DDR4, Storage: 1TB SSD NVMe...");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f2f22222-aaaa-bbbb-cccc-222222222222"),
                column: "Description",
                value: "CPU: Intel Core i5-13400F, GPU: GeForce RTX 3060TI, RAM: 32GB DDR4, Storage: 1TB SSD NVMe...");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f3f33333-aaaa-bbbb-cccc-333333333333"),
                column: "Description",
                value: "CPU: AMD Ryzen 7 5700X3D, GPU: GeForce RTX 4060, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f4f44444-aaaa-bbbb-cccc-444444444444"),
                column: "Description",
                value: "CPU: Intel Core i5-14600KF, GPU: GeForce RTX 5070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f5f55555-aaaa-bbbb-cccc-555555555555"),
                column: "Description",
                value: "CPU: AMD Ryzen 7 7700, GPU: GeForce RTX 5070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...");

            migrationBuilder.UpdateData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f6f66666-aaaa-bbbb-cccc-666666666666"),
                column: "Description",
                value: "CPU: Intel Core i7-13700K, GPU: GeForce RTX 4070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...");
        }
    }
}
