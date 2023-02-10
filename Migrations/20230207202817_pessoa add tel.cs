using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class pessoaaddtel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Pessoas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Pessoas");
        }
    }
}
