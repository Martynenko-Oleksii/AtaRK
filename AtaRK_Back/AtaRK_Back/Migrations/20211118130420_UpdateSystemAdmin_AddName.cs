using Microsoft.EntityFrameworkCore.Migrations;

namespace AtaRK_Back.Migrations
{
    public partial class UpdateSystemAdmin_AddName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SystemAdmins",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "SystemAdmins");
        }
    }
}
