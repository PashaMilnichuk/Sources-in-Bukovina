using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpathianCrown.Api.Migrations
{
    /// <inheritdoc />
    public partial class ServiceItemImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ServiceItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ServiceItems");
        }
    }
}
