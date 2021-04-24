using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum_App.Data.Migrations
{
    public partial class thread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID",
                table: "Post");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Post",
                type: "int",
                nullable: true);
        }
    }
}
