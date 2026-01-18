using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplySoftDeleteForCustodayItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustodyItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustodyItems");
        }
    }
}
