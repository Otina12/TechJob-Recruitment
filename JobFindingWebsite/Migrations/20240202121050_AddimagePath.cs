using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFindingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddimagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Accounts");
        }
    }
}
