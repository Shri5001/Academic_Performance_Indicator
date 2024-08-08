using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicPerformance.Migrations
{
    /// <inheritdoc />
    public partial class Addsemtotheoryresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sem",
                table: "TheoryResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sem",
                table: "TheoryResults");
        }
    }
}
