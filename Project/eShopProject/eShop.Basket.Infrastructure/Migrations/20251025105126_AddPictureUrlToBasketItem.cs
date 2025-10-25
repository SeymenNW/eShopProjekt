using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Basket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPictureUrlToBasketItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "basketitems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "basketitems");
        }
    }
}
