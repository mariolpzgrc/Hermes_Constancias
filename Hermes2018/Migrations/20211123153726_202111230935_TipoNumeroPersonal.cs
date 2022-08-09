using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes2018.Migrations
{
    public partial class _202111230935_TipoNumeroPersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HER_NumeroPersonal",
                table: "HER_InfoUsuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HER_TipoPersonal",
                table: "HER_InfoUsuario",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HER_NumeroPersonal",
                table: "HER_InfoUsuario");

            migrationBuilder.DropColumn(
                name: "HER_TipoPersonal",
                table: "HER_InfoUsuario");
        }
    }
}
