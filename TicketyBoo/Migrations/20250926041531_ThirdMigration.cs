using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketyBoo.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScareType",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "ScareLevel",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScareLevel",
                table: "Category");

            migrationBuilder.AddColumn<string>(
                name: "ScareType",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
