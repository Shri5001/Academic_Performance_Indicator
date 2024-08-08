using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicPerformance.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchYearinEVReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchYear",
                table: "EVReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchYear",
                table: "EVReports");
        }
    }
}
