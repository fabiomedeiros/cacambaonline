using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class CorreçãoNotificacoeseeInfrações : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Cacambas_CacambasId",
                table: "Notificacoes");

            migrationBuilder.RenameColumn(
                name: "CacambasId",
                table: "Notificacoes",
                newName: "CTRId");

            migrationBuilder.RenameColumn(
                name: "AspNetUsersId",
                table: "Notificacoes",
                newName: "UsuarioFiscalId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_CacambasId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_CTRId");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioFiscalId",
                table: "Infracoes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_CTR_CTRId",
                table: "Notificacoes",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_CTR_CTRId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioFiscalId",
                table: "Infracoes");

            migrationBuilder.RenameColumn(
                name: "UsuarioFiscalId",
                table: "Notificacoes",
                newName: "AspNetUsersId");

            migrationBuilder.RenameColumn(
                name: "CTRId",
                table: "Notificacoes",
                newName: "CacambasId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_CTRId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_CacambasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Cacambas_CacambasId",
                table: "Notificacoes",
                column: "CacambasId",
                principalTable: "Cacambas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
