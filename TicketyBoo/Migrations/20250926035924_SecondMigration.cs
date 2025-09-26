using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketyBoo.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Category",
                newName: "ScareType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScareType",
                table: "Category",
                newName: "Location");
        }
    }
}
