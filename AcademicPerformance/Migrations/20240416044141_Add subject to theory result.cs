using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicPerformance.Migrations
{
    /// <inheritdoc />
    public partial class Addsubjecttotheoryresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Top1",
                table: "TheoryResults");

            migrationBuilder.DropColumn(
                name: "Top2",
                table: "TheoryResults");

            migrationBuilder.RenameColumn(
                name: "Top3",
                table: "TheoryResults",
                newName: "ReportData");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "TheoryResults",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_TheoryResults_SubjectId",
                table: "TheoryResults",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TheoryResults_Subjects_SubjectId",
                table: "TheoryResults",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TheoryResults_Subjects_SubjectId",
                table: "TheoryResults");

            migrationBuilder.DropIndex(
                name: "IX_TheoryResults_SubjectId",
                table: "TheoryResults");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TheoryResults");

            migrationBuilder.RenameColumn(
                name: "ReportData",
                table: "TheoryResults",
                newName: "Top3");

            migrationBuilder.AddColumn<string>(
                name: "Top1",
                table: "TheoryResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Top2",
                table: "TheoryResults",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
