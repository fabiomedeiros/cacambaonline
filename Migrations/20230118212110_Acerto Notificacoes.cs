using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class AcertoNotificacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notificacoes_Cacambas_CacambasId",
            //    table: "Notificacoes");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notificacoes_Transportadores_TransportadoresId",
            //    table: "Notificacoes");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notificacoes_CacambasId",
            //    table: "Notificacoes");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notificacoes_TransportadoresId",
            //    table: "Notificacoes");

            //migrationBuilder.DropColumn(
            //    name: "CacambasId",
            //    table: "Notificacoes");

            //migrationBuilder.DropColumn(
            //    name: "TransportadoresId",
            //    table: "Notificacoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CacambasId",
                table: "Notificacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportadoresId",
                table: "Notificacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_CacambasId",
                table: "Notificacoes",
                column: "CacambasId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_TransportadoresId",
                table: "Notificacoes",
                column: "TransportadoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Cacambas_CacambasId",
                table: "Notificacoes",
                column: "CacambasId",
                principalTable: "Cacambas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Transportadores_TransportadoresId",
                table: "Notificacoes",
                column: "TransportadoresId",
                principalTable: "Transportadores",
                principalColumn: "Id");
        }
    }
}
