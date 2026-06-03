using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRecipintsifNoCustodays : Migration
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
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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
                principalColumn: "Id");
        }
    }
}
