using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class addedJsonToOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JSONstring",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JSONstring",
                table: "Order");
        }
    }
}
