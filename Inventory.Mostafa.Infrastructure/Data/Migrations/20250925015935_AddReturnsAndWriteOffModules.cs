using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Mostafa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnsAndWriteOffModules : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    RecipientsId = table.Column<int>(type: "int", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeReleaseItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_Recipients_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Returns_StoreReleaseItems_storeReleaseItemId",
                        column: x => x.storeReleaseItemId,
                        principalTable: "StoreReleaseItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Returns_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WriteOff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnId = table.Column<int>(type: "int", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    RecipintsId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriteOff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WriteOff_Recipients_RecipintsId",
                        column: x => x.RecipintsId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WriteOff_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalTable: "Returns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WriteOff_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Returns_RecipientsId",
                table: "Returns",
                column: "RecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_storeReleaseItemId",
                table: "Returns",
                column: "storeReleaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_UnitId",
                table: "Returns",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteOff_RecipintsId",
                table: "WriteOff",
                column: "RecipintsId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteOff_ReturnId",
                table: "WriteOff",
                column: "ReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteOff_UnitId",
                table: "WriteOff",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WriteOff");

            migrationBuilder.DropTable(
                name: "Returns");
        }
    }
}
