using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaProject.Migrations
{
    public partial class TypoInProductInOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsInOrder_Products_ProductId",
                table: "ProductsInOrder");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductsInOrder");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductsInOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsInOrder_Products_ProductId",
                table: "ProductsInOrder",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsInOrder_Products_ProductId",
                table: "ProductsInOrder");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductsInOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductsInOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsInOrder_Products_ProductId",
                table: "ProductsInOrder",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
