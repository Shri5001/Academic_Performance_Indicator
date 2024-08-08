using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicPerformance.Migrations
{
    /// <inheritdoc />
    public partial class Addcodetosubjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "1234");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Subjects");
        }
    }
}
