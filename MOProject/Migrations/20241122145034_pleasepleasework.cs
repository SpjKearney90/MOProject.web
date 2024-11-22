using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MOProject.Migrations
{
    /// <inheritdoc />
    public partial class pleasepleasework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Settings",
                newName: "Title1");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Settings",
                newName: "ThumbnailUrl1");

            migrationBuilder.RenameColumn(
                name: "SiteName",
                table: "Settings",
                newName: "SiteName1");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Settings",
                newName: "ShortDescription1");

            migrationBuilder.RenameColumn(
                name: "InstagramUrl",
                table: "Settings",
                newName: "InstagramUrl1");

            migrationBuilder.RenameColumn(
                name: "FacebookUrl",
                table: "Settings",
                newName: "FacebookUrl1");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Settings",
                newName: "Id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title1",
                table: "Settings",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl1",
                table: "Settings",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "SiteName1",
                table: "Settings",
                newName: "SiteName");

            migrationBuilder.RenameColumn(
                name: "ShortDescription1",
                table: "Settings",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "InstagramUrl1",
                table: "Settings",
                newName: "InstagramUrl");

            migrationBuilder.RenameColumn(
                name: "FacebookUrl1",
                table: "Settings",
                newName: "FacebookUrl");

            migrationBuilder.RenameColumn(
                name: "Id1",
                table: "Settings",
                newName: "Id");
        }
    }
}
