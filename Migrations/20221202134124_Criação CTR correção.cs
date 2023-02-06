using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class CriaçãoCTRcorreção : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "CTR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Localização = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransportadoresId = table.Column<int>(type: "int", nullable: true),
                    CacambasId = table.Column<int>(type: "int", nullable: true),
                    GeradoresId = table.Column<int>(type: "int", nullable: true),
                    DestinatariosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CTR_Cacambas_CacambasId",
                        column: x => x.CacambasId,
                        principalTable: "Cacambas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_Destinatarios_DestinatariosId",
                        column: x => x.DestinatariosId,
                        principalTable: "Destinatarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_Geradores_GeradoresId",
                        column: x => x.GeradoresId,
                        principalTable: "Geradores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CTR_Transportadores_TransportadoresId",
                        column: x => x.TransportadoresId,
                        principalTable: "Transportadores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTR_CacambasId",
                table: "CTR",
                column: "CacambasId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_DestinatariosId",
                table: "CTR",
                column: "DestinatariosId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_GeradoresId",
                table: "CTR",
                column: "GeradoresId");

            migrationBuilder.CreateIndex(
                name: "IX_CTR_TransportadoresId",
                table: "CTR",
                column: "TransportadoresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTR");

          
        }
    }
}
