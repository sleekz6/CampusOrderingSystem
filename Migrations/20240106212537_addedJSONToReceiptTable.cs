using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class addedJSONToReceiptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JSONForReceipt",
                table: "Receipt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JSONForReceipt",
                table: "Receipt");
        }
    }
}
