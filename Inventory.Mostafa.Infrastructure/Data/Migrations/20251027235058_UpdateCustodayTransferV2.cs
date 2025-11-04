using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustodayTransferV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodayTransfers_Items_ItemId",
                table: "CustodayTransfers");

            migrationBuilder.DropIndex(
                name: "IX_CustodayTransfers_ItemId",
                table: "CustodayTransfers");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "CustodayTransfers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "CustodayTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransfers_ItemId",
                table: "CustodayTransfers",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodayTransfers_Items_ItemId",
                table: "CustodayTransfers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
