using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleProject.Migrations
{
    /// <inheritdoc />
    public partial class AddProductNameArAndEn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Product",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Product",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Category",
                newName: "Name");
        }
    }
}
