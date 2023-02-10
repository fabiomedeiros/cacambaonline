using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class MotivoExclusãoLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Motivo_Exclusao",
                table: "LogNotificacoes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Exclusao",
                table: "LogCTR",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Motivo_Exclusao",
                table: "LogNotificacoes");

            migrationBuilder.DropColumn(
                name: "Motivo_Exclusao",
                table: "LogCTR");
        }
    }
}
