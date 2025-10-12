using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketyBoo.Migrations
{
    /// <inheritdoc />
    public partial class SixthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScareLevel",
                table: "Haunt");

            migrationBuilder.DropColumn(
                name: "ScareLevel",
                table: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScareLevel",
                table: "Haunt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScareLevel",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
