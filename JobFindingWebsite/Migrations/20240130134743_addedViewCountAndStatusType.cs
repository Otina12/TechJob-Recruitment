using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFindingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class addedViewCountAndStatusType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Vacancy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppliedVacancies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppliedVacancies");
        }
    }
}
