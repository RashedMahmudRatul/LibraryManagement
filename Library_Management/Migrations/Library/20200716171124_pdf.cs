using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Management.Migrations.Library
{
    public partial class pdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfBookUrl",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfBookUrl",
                table: "Books");
        }
    }
}
