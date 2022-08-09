using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes2018.Migrations
{
    public partial class AreaUsuarioModificacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HER_BajaInactividad",
                table: "HER_Area",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HER_BajaInactividad",
                table: "HER_Area");
        }
    }
}
