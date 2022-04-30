using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaProject.Migrations
{
    public partial class CorrectDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderFeedbacks_Orders_OrderId1",
                table: "OrderFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_OrderFeedbacks_OrderId1",
                table: "OrderFeedbacks");

            migrationBuilder.DropColumn(
                name: "OrderFeedbackId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderFeedbacks");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFeedbacks_OrderId",
                table: "OrderFeedbacks",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFeedbacks_Orders_OrderId",
                table: "OrderFeedbacks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderFeedbacks_Orders_OrderId",
                table: "OrderFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_OrderFeedbacks_OrderId",
                table: "OrderFeedbacks");

            migrationBuilder.AddColumn<int>(
                name: "OrderFeedbackId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "OrderFeedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderFeedbacks_OrderId1",
                table: "OrderFeedbacks",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFeedbacks_Orders_OrderId1",
                table: "OrderFeedbacks",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
