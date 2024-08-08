using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicPerformance.Migrations
{
    /// <inheritdoc />
    public partial class dropexamtypefromexamsubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamType",
                table: "ExamSubjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamType",
                table: "ExamSubjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
