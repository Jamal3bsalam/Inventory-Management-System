using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class StoreReleaseAndRelatedTablesAndCustodayTablesAndTheRelationsbetweenAll : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSerialNumber_OrderItems_OrderItemsId",
                table: "ItemSerialNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningSerialNumber_OpeningStock_OpeningStockId",
                table: "OpeningSerialNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningStock_Items_ItemsId",
                table: "OpeningStock");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemsId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpeningStock",
                table: "OpeningStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpeningSerialNumber",
                table: "OpeningSerialNumber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemSerialNumber",
                table: "ItemSerialNumber");

            migrationBuilder.RenameTable(
                name: "OpeningStock",
                newName: "OpeningStocks");

            migrationBuilder.RenameTable(
                name: "OpeningSerialNumber",
                newName: "OpeningSerialNumbers");

            migrationBuilder.RenameTable(
                name: "ItemSerialNumber",
                newName: "ItemSerialNumbers");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningStock_ItemsId",
                table: "OpeningStocks",
                newName: "IX_OpeningStocks_ItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningSerialNumber_OpeningStockId",
                table: "OpeningSerialNumbers",
                newName: "IX_OpeningSerialNumbers_OpeningStockId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSerialNumber_OrderItemsId",
                table: "ItemSerialNumbers",
                newName: "IX_ItemSerialNumbers_OrderItemsId");

            migrationBuilder.AddColumn<int>(
                name: "ConsumedQuantity",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReleased",
                table: "ItemSerialNumbers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpeningStocks",
                table: "OpeningStocks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpeningSerialNumbers",
                table: "OpeningSerialNumbers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemSerialNumbers",
                table: "ItemSerialNumbers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Custodays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    RecipientsId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custodays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Custodays_Recipients_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Custodays_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OrderItemsId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BalanceAfter = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RelatedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockTransactions_OrderItems_OrderItemsId",
                        column: x => x.OrderItemsId,
                        principalTable: "OrderItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreReleases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    RecipientsId = table.Column<int>(type: "int", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReleases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreReleases_Recipients_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReleases_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustodyItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustodyId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustodyItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustodyItems_Custodays_CustodyId",
                        column: x => x.CustodyId,
                        principalTable: "Custodays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustodyItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreReleaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreReleaseId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReleaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreReleaseItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReleaseItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReleaseItems_StoreReleases_StoreReleaseId",
                        column: x => x.StoreReleaseId,
                        principalTable: "StoreReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseItemSerialNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreReleaseItemId = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseItemSerialNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseItemSerialNumbers_StoreReleaseItems_StoreReleaseItemId",
                        column: x => x.StoreReleaseItemId,
                        principalTable: "StoreReleaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Custodays_RecipientsId",
                table: "Custodays",
                column: "RecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Custodays_UnitId",
                table: "Custodays",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyItems_CustodyId",
                table: "CustodyItems",
                column: "CustodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyItems_ItemId",
                table: "CustodyItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseItemSerialNumbers_StoreReleaseItemId",
                table: "ReleaseItemSerialNumbers",
                column: "StoreReleaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ItemId",
                table: "StockTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_OrderItemsId",
                table: "StockTransactions",
                column: "OrderItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReleaseItems_ItemId",
                table: "StoreReleaseItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReleaseItems_OrderId",
                table: "StoreReleaseItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReleaseItems_StoreReleaseId",
                table: "StoreReleaseItems",
                column: "StoreReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReleases_RecipientsId",
                table: "StoreReleases",
                column: "RecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReleases_UnitId",
                table: "StoreReleases",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSerialNumbers_OrderItems_OrderItemsId",
                table: "ItemSerialNumbers",
                column: "OrderItemsId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningSerialNumbers_OpeningStocks_OpeningStockId",
                table: "OpeningSerialNumbers",
                column: "OpeningStockId",
                principalTable: "OpeningStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningStocks_Items_ItemsId",
                table: "OpeningStocks",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemsId",
                table: "OrderItems",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSerialNumbers_OrderItems_OrderItemsId",
                table: "ItemSerialNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningSerialNumbers_OpeningStocks_OpeningStockId",
                table: "OpeningSerialNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningStocks_Items_ItemsId",
                table: "OpeningStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemsId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "CustodyItems");

            migrationBuilder.DropTable(
                name: "ReleaseItemSerialNumbers");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Custodays");

            migrationBuilder.DropTable(
                name: "StoreReleaseItems");

            migrationBuilder.DropTable(
                name: "StoreReleases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpeningStocks",
                table: "OpeningStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpeningSerialNumbers",
                table: "OpeningSerialNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemSerialNumbers",
                table: "ItemSerialNumbers");

            migrationBuilder.DropColumn(
                name: "ConsumedQuantity",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsReleased",
                table: "ItemSerialNumbers");

            migrationBuilder.RenameTable(
                name: "OpeningStocks",
                newName: "OpeningStock");

            migrationBuilder.RenameTable(
                name: "OpeningSerialNumbers",
                newName: "OpeningSerialNumber");

            migrationBuilder.RenameTable(
                name: "ItemSerialNumbers",
                newName: "ItemSerialNumber");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningStocks_ItemsId",
                table: "OpeningStock",
                newName: "IX_OpeningStock_ItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningSerialNumbers_OpeningStockId",
                table: "OpeningSerialNumber",
                newName: "IX_OpeningSerialNumber_OpeningStockId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSerialNumbers_OrderItemsId",
                table: "ItemSerialNumber",
                newName: "IX_ItemSerialNumber_OrderItemsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpeningStock",
                table: "OpeningStock",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpeningSerialNumber",
                table: "OpeningSerialNumber",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemSerialNumber",
                table: "ItemSerialNumber",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSerialNumber_OrderItems_OrderItemsId",
                table: "ItemSerialNumber",
                column: "OrderItemsId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningSerialNumber_OpeningStock_OpeningStockId",
                table: "OpeningSerialNumber",
                column: "OpeningStockId",
                principalTable: "OpeningStock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningStock_Items_ItemsId",
                table: "OpeningStock",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemsId",
                table: "OrderItems",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
