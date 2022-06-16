using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etut.Migrations
{
    public partial class added_videoCount_to_courses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoCount",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoCount",
                table: "Courses");
        }
    }
}
