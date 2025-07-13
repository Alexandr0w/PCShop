using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemEntityChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrdersItems",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Foreign key to the referenced Product",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign key to the referenced Product");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrdersItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier for the ordered item");

            migrationBuilder.AddColumn<Guid>(
                name: "ComputerId",
                table: "OrdersItems",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Foreign key to the referenced Computer");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 1,
                comment: "Number of units ordered");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_ComputerId",
                table: "OrdersItems",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_OrderId",
                table: "OrdersItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Computers_ComputerId",
                table: "OrdersItems",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Computers_ComputerId",
                table: "OrdersItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_ComputerId",
                table: "OrdersItems");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_OrderId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrdersItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrdersItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Foreign key to the referenced Product",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "Foreign key to the referenced Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems",
                columns: new[] { "OrderId", "ProductId" });
        }
    }
}
