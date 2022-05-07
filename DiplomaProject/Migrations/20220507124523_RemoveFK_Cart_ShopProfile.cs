using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaProject.Migrations
{
    public partial class RemoveFK_Cart_ShopProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ShopProfiles_ShopProfileId",
                table: "Carts");
            
            migrationBuilder.DropIndex(
                name: "IX_Carts_ShopProfileId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ShopProfileId",
                table: "Carts");
            
            migrationBuilder.AddColumn<int>(
                name: "ShopProfileId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopProfileId",
                table: "Orders",
                column: "ShopProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShopProfiles_ShopProfileId",
                table: "Orders",
                column: "ShopProfileId",
                principalTable: "ShopProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShopProfiles_ShopProfileId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopProfileId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShopProfileId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShopProfileId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ShopProfileId",
                table: "Carts",
                column: "ShopProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ShopProfiles_ShopProfileId",
                table: "Carts",
                column: "ShopProfileId",
                principalTable: "ShopProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
