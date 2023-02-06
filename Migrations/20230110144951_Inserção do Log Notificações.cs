using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class InserçãodoLogNotificações : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notificacoes_Antigas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multa = table.Column<bool>(type: "bit", nullable: true),
                    CTRId = table.Column<int>(type: "int", nullable: false),
                    InfracoesId = table.Column<int>(type: "int", nullable: false),
                    UsuarioFiscalId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes_Antigas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Antigas_CTR_CTRId",
                        column: x => x.CTRId,
                        principalTable: "CTR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Antigas_Infracoes_InfracoesId",
                        column: x => x.InfracoesId,
                        principalTable: "Infracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes_Novas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multa = table.Column<bool>(type: "bit", nullable: true),
                    CTRId = table.Column<int>(type: "int", nullable: false),
                    InfracoesId = table.Column<int>(type: "int", nullable: false),
                    UsuarioFiscalId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes_Novas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Novas_CTR_CTRId",
                        column: x => x.CTRId,
                        principalTable: "CTR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Novas_Infracoes_InfracoesId",
                        column: x => x.InfracoesId,
                        principalTable: "Infracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogNotificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificacoesId = table.Column<int>(type: "int", nullable: true),
                    Notificacoes_AntigoId = table.Column<int>(type: "int", nullable: true),
                    Notificacoes_NovoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogNotificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogNotificacoes_Notificacoes_Antigas_Notificacoes_AntigoId",
                        column: x => x.Notificacoes_AntigoId,
                        principalTable: "Notificacoes_Antigas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogNotificacoes_Notificacoes_NotificacoesId",
                        column: x => x.NotificacoesId,
                        principalTable: "Notificacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogNotificacoes_Notificacoes_Novas_Notificacoes_NovoId",
                        column: x => x.Notificacoes_NovoId,
                        principalTable: "Notificacoes_Novas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogNotificacoes_Notificacoes_AntigoId",
                table: "LogNotificacoes",
                column: "Notificacoes_AntigoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogNotificacoes_Notificacoes_NovoId",
                table: "LogNotificacoes",
                column: "Notificacoes_NovoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogNotificacoes_NotificacoesId",
                table: "LogNotificacoes",
                column: "NotificacoesId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Antigas_CTRId",
                table: "Notificacoes_Antigas",
                column: "CTRId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Antigas_InfracoesId",
                table: "Notificacoes_Antigas",
                column: "InfracoesId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Novas_CTRId",
                table: "Notificacoes_Novas",
                column: "CTRId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Novas_InfracoesId",
                table: "Notificacoes_Novas",
                column: "InfracoesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogNotificacoes");

            migrationBuilder.DropTable(
                name: "Notificacoes_Antigas");

            migrationBuilder.DropTable(
                name: "Notificacoes_Novas");
        }
    }
}
