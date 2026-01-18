using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOneToManyRelationbetweenUnitExpenseItemsandCustodayTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitExpenseItemsId",
                table: "CustodayTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransfers_UnitExpenseItemsId",
                table: "CustodayTransfers",
                column: "UnitExpenseItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodayTransfers_UnitExpenseItems_UnitExpenseItemsId",
                table: "CustodayTransfers",
                column: "UnitExpenseItemsId",
                principalTable: "UnitExpenseItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodayTransfers_UnitExpenseItems_UnitExpenseItemsId",
                table: "CustodayTransfers");

            migrationBuilder.DropIndex(
                name: "IX_CustodayTransfers_UnitExpenseItemsId",
                table: "CustodayTransfers");

            migrationBuilder.DropColumn(
                name: "UnitExpenseItemsId",
                table: "CustodayTransfers");
        }
    }
}
