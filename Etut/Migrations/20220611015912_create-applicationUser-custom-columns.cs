using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etut.Migrations
{
    public partial class createapplicationUsercustomcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userCreationTime",
                table: "AspNetUsers",
                newName: "UserCreationTime");

            migrationBuilder.RenameColumn(
                name: "userCreationDate",
                table: "AspNetUsers",
                newName: "UserCreationDate");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "isAdminApproved",
                table: "AspNetUsers",
                newName: "IsAdminApproved");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "AspNetUsers",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCreationTime",
                table: "AspNetUsers",
                newName: "userCreationTime");

            migrationBuilder.RenameColumn(
                name: "UserCreationDate",
                table: "AspNetUsers",
                newName: "userCreationDate");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "IsAdminApproved",
                table: "AspNetUsers",
                newName: "isAdminApproved");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "firstName");
        }
    }
}
