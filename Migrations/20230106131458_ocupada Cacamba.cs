using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class ocupadaCacamba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UsuarioFiscalId",
                table: "Notificacoes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<int>(
                name: "CTR_antigoId",
                table: "LogCTR",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CTR_novoId",
                table: "LogCTR",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ocupada",
                table: "Cacambas",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CTR_antigo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransportadoresId = table.Column<int>(type: "int", nullable: true),
                    CacambasId = table.Column<int>(type: "int", nullable: true),
                    DestinatariosId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fechada = table.Column<bool>(type: "bit", nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: true),
                    Notificado = table.Column<bool>(type: "bit", nullable: true),
                    Multado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTR_antigo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CTR_antigo_Cacambas_CacambasId",
                        column: x => x.CacambasId,
                        principalTable: "Cacambas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_antigo_Destinatarios_DestinatariosId",
                        column: x => x.DestinatariosId,
                        principalTable: "Destinatarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_antigo_Transportadores_TransportadoresId",
                        column: x => x.TransportadoresId,
                        principalTable: "Transportadores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CTR_novo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransportadoresId = table.Column<int>(type: "int", nullable: true),
                    CacambasId = table.Column<int>(type: "int", nullable: true),
                    DestinatariosId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fechada = table.Column<bool>(type: "bit", nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: true),
                    Notificado = table.Column<bool>(type: "bit", nullable: true),
                    Multado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTR_novo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CTR_novo_Cacambas_CacambasId",
                        column: x => x.CacambasId,
                        principalTable: "Cacambas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_novo_Destinatarios_DestinatariosId",
                        column: x => x.DestinatariosId,
                        principalTable: "Destinatarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_novo_Transportadores_TransportadoresId",
                        column: x => x.TransportadoresId,
                        principalTable: "Transportadores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogCTR_CTR_antigoId",
                table: "LogCTR",
                column: "CTR_antigoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogCTR_CTR_novoId",
                table: "LogCTR",
                column: "CTR_novoId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_antigo_CacambasId",
                table: "CTR_antigo",
                column: "CacambasId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_antigo_DestinatariosId",
                table: "CTR_antigo",
                column: "DestinatariosId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_antigo_TransportadoresId",
                table: "CTR_antigo",
                column: "TransportadoresId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_novo_CacambasId",
                table: "CTR_novo",
                column: "CacambasId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_novo_DestinatariosId",
                table: "CTR_novo",
                column: "DestinatariosId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_novo_TransportadoresId",
                table: "CTR_novo",
                column: "TransportadoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogCTR_CTR_antigo_CTR_antigoId",
                table: "LogCTR",
                column: "CTR_antigoId",
                principalTable: "CTR_antigo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogCTR_CTR_novo_CTR_novoId",
                table: "LogCTR",
                column: "CTR_novoId",
                principalTable: "CTR_novo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogCTR_CTR_antigo_CTR_antigoId",
                table: "LogCTR");

            migrationBuilder.DropForeignKey(
                name: "FK_LogCTR_CTR_novo_CTR_novoId",
                table: "LogCTR");

            migrationBuilder.DropTable(
                name: "CTR_antigo");

            migrationBuilder.DropTable(
                name: "CTR_novo");

            migrationBuilder.DropIndex(
                name: "IX_LogCTR_CTR_antigoId",
                table: "LogCTR");

            migrationBuilder.DropIndex(
                name: "IX_LogCTR_CTR_novoId",
                table: "LogCTR");

            migrationBuilder.DropColumn(
                name: "CTR_antigoId",
                table: "LogCTR");

            migrationBuilder.DropColumn(
                name: "CTR_novoId",
                table: "LogCTR");

            migrationBuilder.DropColumn(
                name: "Ocupada",
                table: "Cacambas");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioFiscalId",
                table: "Notificacoes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);
        }
    }
}
