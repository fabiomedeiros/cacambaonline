using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class Datananotificacaoacerto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Notificacoes");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Notificacoes",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Notificacoes");

            migrationBuilder.AddColumn<bool>(
                name: "DataCriacao",
                table: "Notificacoes",
                type: "bit",
                nullable: true);
        }
    }
}
