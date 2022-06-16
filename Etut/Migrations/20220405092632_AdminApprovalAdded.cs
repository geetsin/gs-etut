using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etut.Migrations
{
    public partial class AdminApprovalAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdminApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdminApproved",
                table: "AspNetUsers");
        }
    }
}
