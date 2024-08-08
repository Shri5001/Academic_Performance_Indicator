using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicPerformance.Migrations
{
    /// <inheritdoc />
    public partial class Addsubjecttoevreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "EVReports",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_EVReports_SubjectId",
                table: "EVReports",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EVReports_Subjects_SubjectId",
                table: "EVReports",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EVReports_Subjects_SubjectId",
                table: "EVReports");

            migrationBuilder.DropIndex(
                name: "IX_EVReports_SubjectId",
                table: "EVReports");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "EVReports");
        }
    }
}
