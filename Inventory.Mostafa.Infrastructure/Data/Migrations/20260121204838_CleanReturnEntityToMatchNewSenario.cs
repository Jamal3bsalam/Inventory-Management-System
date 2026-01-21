using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CleanReturnEntityToMatchNewSenario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Items_ItemId",
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

            migrationBuilder.DropColumn(
                name: "WriteOfQuantity",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Returns",
                newName: "UnitExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_UnitExpenseId",
                table: "Returns",
                column: "UnitExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_UnitExpense_UnitExpenseId",
                table: "Returns",
                column: "UnitExpenseId",
                principalTable: "UnitExpense",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_UnitExpense_UnitExpenseId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_UnitExpenseId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "UnitExpenseId",
                table: "Returns",
                newName: "Quantity");

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

            migrationBuilder.AddColumn<int>(
                name: "WriteOfQuantity",
                table: "Returns",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "FK_Returns_UnitExpense_ExpenseId",
                table: "Returns",
                column: "ExpenseId",
                principalTable: "UnitExpense",
                principalColumn: "Id");
        }
    }
}
