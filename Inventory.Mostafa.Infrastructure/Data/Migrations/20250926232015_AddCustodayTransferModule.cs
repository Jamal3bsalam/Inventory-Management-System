using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustodayTransferModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustodayTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OldRecipientId = table.Column<int>(type: "int", nullable: false),
                    NewRecipientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustodayTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustodayTransfers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustodayTransfers_Recipients_NewRecipientId",
                        column: x => x.NewRecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustodayTransfers_Recipients_OldRecipientId",
                        column: x => x.OldRecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustodayTransfers_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransfers_ItemId",
                table: "CustodayTransfers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransfers_NewRecipientId",
                table: "CustodayTransfers",
                column: "NewRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransfers_OldRecipientId",
                table: "CustodayTransfers",
                column: "OldRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodayTransfers_UnitId",
                table: "CustodayTransfers",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustodayTransfers");
        }
    }
}
