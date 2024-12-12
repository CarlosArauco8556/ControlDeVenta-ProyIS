using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlDeVenta_Proy.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class supplyKeysChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplies",
                table: "Supplies");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Supplies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplies",
                table: "Supplies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_ProductId",
                table: "Supplies",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplies",
                table: "Supplies");

            migrationBuilder.DropIndex(
                name: "IX_Supplies_ProductId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Supplies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplies",
                table: "Supplies",
                columns: new[] { "ProductId", "SupplierId" });
        }
    }
}
