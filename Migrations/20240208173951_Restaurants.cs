using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusOrdering.Migrations
{
    /// <inheritdoc />
    public partial class Restaurants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Restaurant (Name, LogoUrl) VALUES ('ChickFilA', '../images/chick.png')");
            migrationBuilder.Sql("INSERT INTO Restaurant (Name, LogoUrl) VALUES ('Panda Express', '../images/panda.png')");
            migrationBuilder.Sql("INSERT INTO Restaurant (Name, LogoUrl) VALUES ('Einstein Bros. Bagels', '../images/einstein.png')");
            migrationBuilder.Sql("INSERT INTO MenuItem (ItemName, Price, RestaurantId, ImageUrl, Calories, Size) VALUES ('Nuggets', '3.99', '1', '../images/nuggets.png', '400', 'small')");
            migrationBuilder.Sql("INSERT INTO Customer (Name, Birthdate, Email, Password) VALUES ('Amy', '01/05/2003', amy@gmail.com, Amy123)");
            migrationBuilder.Sql("INSERT INTO Customer (Name, Birthdate, Email, Password) VALUES ('Jason', '10/15/1993', jason@gmail.com, Jason123)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
