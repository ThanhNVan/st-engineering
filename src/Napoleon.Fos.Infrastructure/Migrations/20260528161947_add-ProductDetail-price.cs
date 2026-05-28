using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Napoleon.Fos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addProductDetailprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Varience",
                table: "ProductDetails",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "ProductDetails",
                type: "int",
                maxLength: 36,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Varience",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);
        }  
    }
}
