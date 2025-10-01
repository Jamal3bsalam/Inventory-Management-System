using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnitExpenseTablesAndRelations : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "StoreReleaseItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UnitExpense",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    RecipientsId = table.Column<int>(type: "int", nullable: true),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitExpense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitExpense_Recipients_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnitExpense_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnitExpenseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitExpenseId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitExpenseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitExpenseItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnitExpenseItems_UnitExpense_UnitExpenseId",
                        column: x => x.UnitExpenseId,
                        principalTable: "UnitExpense",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitExpense_RecipientsId",
                table: "UnitExpense",
                column: "RecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitExpense_UnitId",
                table: "UnitExpense",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitExpenseItems_ItemId",
                table: "UnitExpenseItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitExpenseItems_UnitExpenseId",
                table: "UnitExpenseItems",
                column: "UnitExpenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitExpenseItems");

            migrationBuilder.DropTable(
                name: "UnitExpense");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "StoreReleaseItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
