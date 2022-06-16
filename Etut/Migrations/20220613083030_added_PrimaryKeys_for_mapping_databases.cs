using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etut.Migrations
{
    public partial class added_PrimaryKeys_for_mapping_databases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserCourses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CourseVideos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseVideos",
                table: "CourseVideos",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseVideos",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseVideos");
        }
    }
}
