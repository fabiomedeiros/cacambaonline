using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class addativaNotifivações : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativa",
                table: "Notificacoes",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativa",
                table: "Notificacoes");
        }
    }
}
