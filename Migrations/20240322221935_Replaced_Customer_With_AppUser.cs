using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class Replaced_Customer_With_AppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "purchasingUserId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_purchasingUserId",
                table: "Order",
                column: "purchasingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_purchasingUserId",
                table: "Order",
                column: "purchasingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_purchasingUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_purchasingUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "purchasingUserId",
                table: "Order");
        }
    }
}
