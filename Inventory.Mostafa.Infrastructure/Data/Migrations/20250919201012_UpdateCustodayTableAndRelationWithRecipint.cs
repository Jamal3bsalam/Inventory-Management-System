using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustodayTableAndRelationWithRecipint : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Custodays_RecipientsId",
                table: "Custodays");

            migrationBuilder.AddColumn<int>(
                name: "CustodayId",
                table: "Recipients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Custodays_RecipientsId",
                table: "Custodays",
                column: "RecipientsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Custodays_RecipientsId",
                table: "Custodays");

            migrationBuilder.DropColumn(
                name: "CustodayId",
                table: "Recipients");

            migrationBuilder.CreateIndex(
                name: "IX_Custodays_RecipientsId",
                table: "Custodays",
                column: "RecipientsId");
        }
    }
}
