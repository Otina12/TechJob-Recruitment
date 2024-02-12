using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFindingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class addedLinkedInURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedInURL",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedInURL",
                table: "Accounts");
        }
    }
}
