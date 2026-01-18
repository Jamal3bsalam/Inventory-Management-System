using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRelationFromCustodayItemandUnitExpenseItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustodyItemUnitExpense",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustodyItemId = table.Column<int>(type: "int", nullable: true),
                    UnitExpenseItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustodyItemUnitExpense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustodyItemUnitExpense_CustodyItems_CustodyItemId",
                        column: x => x.CustodyItemId,
                        principalTable: "CustodyItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustodyItemUnitExpense_UnitExpenseItems_UnitExpenseItemId",
                        column: x => x.UnitExpenseItemId,
                        principalTable: "UnitExpenseItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustodyItemUnitExpense_CustodyItemId",
                table: "CustodyItemUnitExpense",
                column: "CustodyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyItemUnitExpense_UnitExpenseItemId",
                table: "CustodyItemUnitExpense",
                column: "UnitExpenseItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustodyItemUnitExpense");
        }
    }
}
