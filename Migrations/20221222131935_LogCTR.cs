using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class LogCTR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogCTR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTRId = table.Column<int>(type: "int", nullable: true),
                    Operacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogCTR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogCTR_CTR_CTRId",
                        column: x => x.CTRId,
                        principalTable: "CTR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogCTR_CTRId",
                table: "LogCTR",
                column: "CTRId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogCTR");
        }
    }
}
