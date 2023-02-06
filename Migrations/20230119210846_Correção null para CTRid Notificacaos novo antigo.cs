using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class CorreçãonullparaCTRidNotificacaosnovoantigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_CTR_CTRId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Antigas_CTR_CTRId",
                table: "Notificacoes_Antigas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Novas_CTR_CTRId",
                table: "Notificacoes_Novas");

            migrationBuilder.AlterColumn<int>(
                name: "CTRId",
                table: "Notificacoes_Novas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CTRId",
                table: "Notificacoes_Antigas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CTRId",
                table: "Notificacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_CTR_CTRId",
                table: "Notificacoes",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Antigas_CTR_CTRId",
                table: "Notificacoes_Antigas",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Novas_CTR_CTRId",
                table: "Notificacoes_Novas",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_CTR_CTRId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Antigas_CTR_CTRId",
                table: "Notificacoes_Antigas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Novas_CTR_CTRId",
                table: "Notificacoes_Novas");

            migrationBuilder.AlterColumn<int>(
                name: "CTRId",
                table: "Notificacoes_Novas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CTRId",
                table: "Notificacoes_Antigas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CTRId",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_CTR_CTRId",
                table: "Notificacoes",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Antigas_CTR_CTRId",
                table: "Notificacoes_Antigas",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Novas_CTR_CTRId",
                table: "Notificacoes_Novas",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
