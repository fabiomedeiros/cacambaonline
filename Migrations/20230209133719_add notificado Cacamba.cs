using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class addnotificadoCacamba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Notificado",
                table: "Cacambas",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notificado",
                table: "Cacambas");
        }
    }
}
