using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class CriaçãodastabelasUsuarioTransportadoreUsuarioIdDestinatario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioDestinatarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinatariosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioDestinatarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioDestinatarios_Destinatarios_DestinatariosId",
                        column: x => x.DestinatariosId,
                        principalTable: "Destinatarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioTransportadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportadoresId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioTransportadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioTransportadores_Transportadores_TransportadoresId",
                        column: x => x.TransportadoresId,
                        principalTable: "Transportadores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioDestinatarios_DestinatariosId",
                table: "UsuarioDestinatarios",
                column: "DestinatariosId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioTransportadores_TransportadoresId",
                table: "UsuarioTransportadores",
                column: "TransportadoresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioDestinatarios");

            migrationBuilder.DropTable(
                name: "UsuarioTransportadores");
        }
    }
}
