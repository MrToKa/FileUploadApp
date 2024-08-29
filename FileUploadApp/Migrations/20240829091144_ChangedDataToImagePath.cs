using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileUploadApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDataToImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Images");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Images",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
