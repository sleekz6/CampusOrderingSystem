using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class Added_GuestName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestName",
                table: "Order");
        }
    }
}
