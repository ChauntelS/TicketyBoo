using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketyBoo.Migrations
{
    /// <inheritdoc />
    public partial class FithMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Creation",
                table: "Haunt",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Haunt",
                newName: "Creation");
        }
    }
}
