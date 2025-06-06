using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMOON.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRatingToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRating",
                table: "Orders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRating",
                table: "Orders");
        }
    }
}
