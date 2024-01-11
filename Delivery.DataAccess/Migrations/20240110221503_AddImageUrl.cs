using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAll.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 13);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FoodItems",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 13, 2, "1l", "TOMATO JUICE", 62.0 });
        }
    }
}
