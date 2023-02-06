using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class CorreçãoretiradaGeradoresdoCTR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CTR_Geradores_GeradoresId",
                table: "CTR");

            migrationBuilder.DropIndex(
                name: "IX_CTR_GeradoresId",
                table: "CTR");

            migrationBuilder.DropColumn(
                name: "GeradoresId",
                table: "CTR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeradoresId",
                table: "CTR",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CTR_GeradoresId",
                table: "CTR",
                column: "GeradoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_CTR_Geradores_GeradoresId",
                table: "CTR",
                column: "GeradoresId",
                principalTable: "Geradores",
                principalColumn: "Id");
        }
    }
}
