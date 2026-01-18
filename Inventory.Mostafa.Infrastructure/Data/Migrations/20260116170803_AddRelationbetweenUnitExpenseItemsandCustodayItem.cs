using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationbetweenUnitExpenseItemsandCustodayItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitExpenseItemsId",
                table: "CustodyItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustodyItems_UnitExpenseItemsId",
                table: "CustodyItems",
                column: "UnitExpenseItemsId",
                unique: true,
                filter: "[UnitExpenseItemsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyItems_UnitExpenseItems_UnitExpenseItemsId",
                table: "CustodyItems",
                column: "UnitExpenseItemsId",
                principalTable: "UnitExpenseItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodyItems_UnitExpenseItems_UnitExpenseItemsId",
                table: "CustodyItems");

            migrationBuilder.DropIndex(
                name: "IX_CustodyItems_UnitExpenseItemsId",
                table: "CustodyItems");

            migrationBuilder.DropColumn(
                name: "UnitExpenseItemsId",
                table: "CustodyItems");
        }
    }
}
