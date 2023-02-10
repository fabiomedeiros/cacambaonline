using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cacambaonline.Migrations
{
    public partial class acertodatabaixactr_antigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataBaixa",
                table: "CTR_antigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataBaixa",
                table: "CTR_antigo",
                type: "datetime2",
                nullable: true);
        }
    }
}
