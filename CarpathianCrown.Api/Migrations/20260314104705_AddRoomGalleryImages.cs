using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpathianCrown.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomGalleryImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4",
                table: "Rooms",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Image4",
                table: "Rooms");
        }
    }
}
