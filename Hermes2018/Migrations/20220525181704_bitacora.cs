using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Hermes2018.Migrations
{
    public partial class bitacora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HER_Bitacora",
                columns: table => new
                {
                    HER_BitacoraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Operacion = table.Column<string>(nullable: true),
                    HER_Ip = table.Column<string>(nullable: true),
                    HER_Fecha = table.Column<DateTime>(nullable: false),
                    HER_UserName = table.Column<string>(nullable: true),
                    HER_NombreUsuario = table.Column<string>(nullable: true),
                    HER_InfoUsuarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Bitacora", x => x.HER_BitacoraId);
                    table.ForeignKey(
                        name: "FK_HER_Bitacora_HER_InfoUsuario_HER_InfoUsuarioId",
                        column: x => x.HER_InfoUsuarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HER_Bitacora_HER_InfoUsuarioId",
                table: "HER_Bitacora",
                column: "HER_InfoUsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HER_Bitacora");
        }
    }
}
