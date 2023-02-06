using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class InfracoesnaNotificacão : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InfracoesId",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_InfracoesId",
                table: "Notificacoes",
                column: "InfracoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Infracoes_InfracoesId",
                table: "Notificacoes",
                column: "InfracoesId",
                principalTable: "Infracoes",
                principalColumn: "Id",
                //onDelete: ReferentialAction.Cascade);
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Infracoes_InfracoesId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_InfracoesId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "InfracoesId",
                table: "Notificacoes");
        }
    }
}
