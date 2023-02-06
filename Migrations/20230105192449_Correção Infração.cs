using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class CorreçãoInfração : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Infracoes_CTR_CTRId",
                table: "Infracoes");

            migrationBuilder.DropIndex(
                name: "IX_Infracoes_CTRId",
                table: "Infracoes");

            migrationBuilder.DropColumn(
                name: "CTRId",
                table: "Infracoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CTRId",
                table: "Infracoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Infracoes_CTRId",
                table: "Infracoes",
                column: "CTRId");

            migrationBuilder.AddForeignKey(
                name: "FK_Infracoes_CTR_CTRId",
                table: "Infracoes",
                column: "CTRId",
                principalTable: "CTR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
