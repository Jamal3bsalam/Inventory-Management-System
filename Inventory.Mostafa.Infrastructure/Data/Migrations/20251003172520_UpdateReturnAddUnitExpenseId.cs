using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReturnAddUnitExpenseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_StoreReleaseItems_storeReleaseItemId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "storeReleaseItemId",
                table: "Returns",
                newName: "StoreReleaseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Returns_storeReleaseItemId",
                table: "Returns",
                newName: "IX_Returns_StoreReleaseItemId");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "StoreReleaseItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "Returns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Returns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ExpenseId",
                table: "Returns",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ItemId",
                table: "Returns",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Items_ItemId",
                table: "Returns",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_StoreReleaseItems_StoreReleaseItemId",
                table: "Returns",
                column: "StoreReleaseItemId",
                principalTable: "StoreReleaseItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_UnitExpense_ExpenseId",
                table: "Returns",
                column: "ExpenseId",
                principalTable: "UnitExpense",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Items_ItemId",
                table: "Returns");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_StoreReleaseItems_StoreReleaseItemId",
                table: "Returns");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_UnitExpense_ExpenseId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_ExpenseId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_ItemId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "StoreReleaseItemId",
                table: "Returns",
                newName: "storeReleaseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Returns_StoreReleaseItemId",
                table: "Returns",
                newName: "IX_Returns_storeReleaseItemId");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "StoreReleaseItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_StoreReleaseItems_storeReleaseItemId",
                table: "Returns",
                column: "storeReleaseItemId",
                principalTable: "StoreReleaseItems",
                principalColumn: "Id");
        }
    }
}
