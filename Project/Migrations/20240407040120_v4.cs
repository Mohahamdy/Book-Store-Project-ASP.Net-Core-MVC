using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Discounts_Discount_id",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "Discount_id",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Discounts_Discount_id",
                table: "Books",
                column: "Discount_id",
                principalTable: "Discounts",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Discounts_Discount_id",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "Discount_id",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Discounts_Discount_id",
                table: "Books",
                column: "Discount_id",
                principalTable: "Discounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
