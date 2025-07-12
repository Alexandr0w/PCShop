using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class DefaultUserFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PostalCode", "SecurityStamp" },
                values: new object[] { "20156ffd-ca5b-493f-8cdb-f0e37d6ffc28", "AQAAAAIAAYagAAAAEN6HKPt1nIvfpyU80C1hHBdt/MBp8t8qhTJqbc55PHUBGvA9g2Y47ybCLa5zyIrOPg==", "0000", "cdd1077d-f80c-4f64-b3bc-e7b71fc4ef9a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PostalCode", "SecurityStamp" },
                values: new object[] { "0d516785-0a7d-4072-9778-e26e13a61503", "AQAAAAIAAYagAAAAEDrwOD89DM2ltbGCB9X/IFamXNCgtz1WrTuHk05WpUx4pROfIz9A0OJKKjpk3WGfgQ==", "00000", "1d948447-fa3e-448d-b4b4-54a0b207d0ea" });
        }
    }
}
