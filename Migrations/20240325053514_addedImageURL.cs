using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class addedImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "CartItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "CartItem");
        }
    }
}
