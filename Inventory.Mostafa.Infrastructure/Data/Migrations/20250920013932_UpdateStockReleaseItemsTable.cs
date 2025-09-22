using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStockReleaseItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentNumber",
                table: "StoreReleases",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "StoreReleaseItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreReleaseItems_OrderItemId",
                table: "StoreReleaseItems",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreReleaseItems_OrderItems_OrderItemId",
                table: "StoreReleaseItems",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreReleaseItems_OrderItems_OrderItemId",
                table: "StoreReleaseItems");

            migrationBuilder.DropIndex(
                name: "IX_StoreReleaseItems_OrderItemId",
                table: "StoreReleaseItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "StoreReleaseItems");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentNumber",
                table: "StoreReleases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
