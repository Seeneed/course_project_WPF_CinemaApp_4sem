using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMOON.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToMovieAndSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Movies");
        }
    }
}
