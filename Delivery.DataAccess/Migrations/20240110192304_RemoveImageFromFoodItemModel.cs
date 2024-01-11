using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAll.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageFromFoodItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "FoodItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "ImageUrl",
                value: "");
        }
    }
}
