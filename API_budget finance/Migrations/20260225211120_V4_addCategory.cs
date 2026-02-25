using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_budget_finance.Migrations
{
    /// <inheritdoc />
    public partial class V4_addCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Argents");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Argents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Argents_CategoryId",
                table: "Argents",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Argents_Categories_CategoryId",
                table: "Argents",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Argents_Categories_CategoryId",
                table: "Argents");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Argents_CategoryId",
                table: "Argents");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Argents");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Argents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
