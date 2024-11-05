using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MOProject.Migrations
{
    /// <inheritdoc />
    public partial class ThumbnailUrlAded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Posts");
        }
    }
}
