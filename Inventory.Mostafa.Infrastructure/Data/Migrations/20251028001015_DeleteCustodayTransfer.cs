using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCustodayTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustodayTransferItem");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "CustodayTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CustodayTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CustodayTransfers");

            migrationBuilder.CreateTable(
                name: "CustodayTransferItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustodayTransfersId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustodayTransferItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustodayTransferItem_CustodayTransfers_CustodayTransfersId",
                        column: x => x.CustodayTransfersId,
                        principalTable: "CustodayTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustodayTransferItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransferItem_CustodayTransfersId",
                table: "CustodayTransferItem",
                column: "CustodayTransfersId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransferItem_ItemId",
                table: "CustodayTransferItem",
                column: "ItemId");
        }
    }
}
