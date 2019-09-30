using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalRandkowy.Migrations
{
    public partial class ExtendedUserFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Users",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Users",
                newName: "Decription");
        }
    }
}
