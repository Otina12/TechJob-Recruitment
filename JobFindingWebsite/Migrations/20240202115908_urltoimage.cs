using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFindingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class urltoimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
