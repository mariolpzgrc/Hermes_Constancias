using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes2018.Migrations
{
    public partial class CambioEnvio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HER_EsOculto",
                table: "HER_Envio",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HER_EsOculto",
                table: "HER_Envio");
        }
    }
}
