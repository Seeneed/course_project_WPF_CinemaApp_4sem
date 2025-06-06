using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaMOON.Migrations
{
    /// <inheritdoc />
    public partial class SeedHallData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "Capacity", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("14ec82a0-460f-4be2-9bc9-1c56e949a17e"), 70, "HallPage_LargeHallName", "large" },
                    { new Guid("b68871d6-35e5-432c-b0e0-3178586333e9"), 60, "HallPage_MediumHallName", "medium" },
                    { new Guid("da291a5e-71c2-46a6-965c-02d3e2c59431"), 45, "HallPage_SmallHallName", "small" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: new Guid("14ec82a0-460f-4be2-9bc9-1c56e949a17e"));

            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: new Guid("b68871d6-35e5-432c-b0e0-3178586333e9"));

            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: new Guid("da291a5e-71c2-46a6-965c-02d3e2c59431"));
        }
    }
}
