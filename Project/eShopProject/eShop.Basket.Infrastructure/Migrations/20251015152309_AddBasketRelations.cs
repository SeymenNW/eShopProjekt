using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Basket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBasketRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Baskets_ShoppingBasketId",
                table: "BasketItems");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Baskets_ShoppingBasketId",
                table: "BasketItems",
                column: "ShoppingBasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Baskets_ShoppingBasketId",
                table: "BasketItems");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Baskets_ShoppingBasketId",
                table: "BasketItems",
                column: "ShoppingBasketId",
                principalTable: "Baskets",
                principalColumn: "Id");
        }
    }
}
