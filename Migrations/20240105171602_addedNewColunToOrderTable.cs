using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class addedNewColunToOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isServed",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isServed",
                table: "Order");
        }
    }
}
