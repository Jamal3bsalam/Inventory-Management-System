using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UndoDeleteRecipintsifNoCustodays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitExpense_Recipients_RecipientsId",
                table: "UnitExpense");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitExpense_Recipients_RecipientsId",
                table: "UnitExpense",
                column: "RecipientsId",
                principalTable: "Recipients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitExpense_Recipients_RecipientsId",
                table: "UnitExpense");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitExpense_Recipients_RecipientsId",
                table: "UnitExpense",
                column: "RecipientsId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
