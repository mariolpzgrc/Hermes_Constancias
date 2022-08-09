using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes2018.Migrations
{
    public partial class PrincipalMar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    HER_NombreCompleto = table.Column<string>(nullable: true),
                    HER_Aprobado = table.Column<bool>(nullable: true),
                    HER_FechaAprobado = table.Column<DateTime>(nullable: true),
                    HER_AceptoTerminos = table.Column<bool>(nullable: true),
                    HER_FechaAceptoTerminos = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HER_Anexo",
                columns: table => new
                {
                    HER_AnexoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Tipo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Anexo", x => x.HER_AnexoId);
                });

            migrationBuilder.CreateTable(
                name: "HER_AnexoGeneral",
                columns: table => new
                {
                    HER_AnexoGeneralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_RutaComplemento = table.Column<string>(nullable: true),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_TipoContenido = table.Column<string>(nullable: true),
                    HER_TipoRegistro = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_AnexoGeneral", x => x.HER_AnexoGeneralId);
                });

            migrationBuilder.CreateTable(
                name: "HER_AnexoRuta",
                columns: table => new
                {
                    HER_AnexoRutaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_RutaBase = table.Column<string>(nullable: true),
                    HER_Estado = table.Column<int>(nullable: false),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_FechaActualizacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_AnexoRuta", x => x.HER_AnexoRutaId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Calendario",
                columns: table => new
                {
                    HER_CalendarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Titulo = table.Column<string>(nullable: true),
                    HER_Anio = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Calendario", x => x.HER_CalendarioId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Color",
                columns: table => new
                {
                    HER_ColorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_Codigo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Color", x => x.HER_ColorId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Configuracion",
                columns: table => new
                {
                    HER_ConfiguracionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Propiedad = table.Column<string>(nullable: true),
                    HER_Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Configuracion", x => x.HER_ConfiguracionId);
                });

            migrationBuilder.CreateTable(
                name: "HER_EstadoDocumento",
                columns: table => new
                {
                    HER_EstadoDocumentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_EstadoDocumento", x => x.HER_EstadoDocumentoId);
                });

            migrationBuilder.CreateTable(
                name: "HER_EstadoEnvio",
                columns: table => new
                {
                    HER_EstadoEnvioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_EstadoEnvio", x => x.HER_EstadoEnvioId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Extension",
                columns: table => new
                {
                    HER_ExtensionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Extension", x => x.HER_ExtensionId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Importancia",
                columns: table => new
                {
                    HER_ImportanciaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Importancia", x => x.HER_ImportanciaId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Region",
                columns: table => new
                {
                    HER_RegionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Region", x => x.HER_RegionId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Rol",
                columns: table => new
                {
                    HER_RolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Rol", x => x.HER_RolId);
                });

            migrationBuilder.CreateTable(
                name: "HER_TipoDocumento",
                columns: table => new
                {
                    HER_TipoDocumentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_TipoDocumento", x => x.HER_TipoDocumentoId);
                });

            migrationBuilder.CreateTable(
                name: "HER_TipoEnvio",
                columns: table => new
                {
                    HER_TipoEnvioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_TipoEnvio", x => x.HER_TipoEnvioId);
                });

            migrationBuilder.CreateTable(
                name: "HER_Visibilidad",
                columns: table => new
                {
                    HER_VisibilidadId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Visibilidad", x => x.HER_VisibilidadId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HER_AnexoArchivo",
                columns: table => new
                {
                    HER_AnexoArchivoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_RutaComplemento = table.Column<string>(nullable: true),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_TipoContenido = table.Column<string>(nullable: true),
                    HER_AnexoId = table.Column<int>(nullable: false),
                    HER_AnexoRutaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_AnexoArchivo", x => x.HER_AnexoArchivoId);
                    table.ForeignKey(
                        name: "FK_HER_AnexoArchivo_HER_Anexo_HER_AnexoId",
                        column: x => x.HER_AnexoId,
                        principalTable: "HER_Anexo",
                        principalColumn: "HER_AnexoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_AnexoArchivo_HER_AnexoRuta_HER_AnexoRutaId",
                        column: x => x.HER_AnexoRutaId,
                        principalTable: "HER_AnexoRuta",
                        principalColumn: "HER_AnexoRutaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_AnexoArea",
                columns: table => new
                {
                    HER_AnexoAreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_RutaComplemento = table.Column<string>(nullable: true),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_TipoContenido = table.Column<string>(nullable: true),
                    HER_AnexoRutaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_AnexoArea", x => x.HER_AnexoAreaId);
                    table.ForeignKey(
                        name: "FK_HER_AnexoArea_HER_AnexoRuta_HER_AnexoRutaId",
                        column: x => x.HER_AnexoRutaId,
                        principalTable: "HER_AnexoRuta",
                        principalColumn: "HER_AnexoRutaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_CalendarioContenido",
                columns: table => new
                {
                    HER_CalendarioContenidoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Mes = table.Column<int>(nullable: false),
                    HER_Dia = table.Column<int>(nullable: false),
                    HER_Fecha = table.Column<DateTime>(nullable: false),
                    HER_CalendarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_CalendarioContenido", x => x.HER_CalendarioContenidoId);
                    table.ForeignKey(
                        name: "FK_HER_CalendarioContenido_HER_Calendario_HER_CalendarioId",
                        column: x => x.HER_CalendarioId,
                        principalTable: "HER_Calendario",
                        principalColumn: "HER_CalendarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_ConfiguracionUsuario",
                columns: table => new
                {
                    HER_ConfiguracionUsuarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Notificaciones = table.Column<bool>(nullable: false),
                    HER_ElementosPorPagina = table.Column<int>(nullable: false),
                    HER_UsuarioId = table.Column<string>(nullable: true),
                    HER_ColorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_ConfiguracionUsuario", x => x.HER_ConfiguracionUsuarioId);
                    table.ForeignKey(
                        name: "FK_HER_ConfiguracionUsuario_HER_Color_HER_ColorId",
                        column: x => x.HER_ColorId,
                        principalTable: "HER_Color",
                        principalColumn: "HER_ColorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_ConfiguracionUsuario_AspNetUsers_HER_UsuarioId",
                        column: x => x.HER_UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Servicio",
                columns: table => new
                {
                    HER_ServicioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_CreadorId = table.Column<string>(nullable: true),
                    HER_RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Servicio", x => x.HER_ServicioId);
                    table.ForeignKey(
                        name: "FK_HER_Servicio_AspNetUsers_HER_CreadorId",
                        column: x => x.HER_CreadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Servicio_HER_Region_HER_RegionId",
                        column: x => x.HER_RegionId,
                        principalTable: "HER_Region",
                        principalColumn: "HER_RegionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Area",
                columns: table => new
                {
                    HER_AreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_Clave = table.Column<string>(nullable: true),
                    HER_DiasCompromiso = table.Column<int>(nullable: false),
                    HER_Direccion = table.Column<string>(nullable: true),
                    HER_Telefono = table.Column<string>(nullable: true),
                    HER_Visible = table.Column<bool>(nullable: false),
                    HER_Estado = table.Column<int>(nullable: false),
                    HER_ExisteEnSIIU = table.Column<bool>(nullable: false),
                    HER_RegionId = table.Column<int>(nullable: false),
                    HER_Area_PadreId = table.Column<int>(nullable: true),
                    HER_LogoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Area", x => x.HER_AreaId);
                    table.ForeignKey(
                        name: "FK_HER_Area_HER_Area_HER_Area_PadreId",
                        column: x => x.HER_Area_PadreId,
                        principalTable: "HER_Area",
                        principalColumn: "HER_AreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Area_HER_AnexoArea_HER_LogoId",
                        column: x => x.HER_LogoId,
                        principalTable: "HER_AnexoArea",
                        principalColumn: "HER_AnexoAreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Area_HER_Region_HER_RegionId",
                        column: x => x.HER_RegionId,
                        principalTable: "HER_Region",
                        principalColumn: "HER_RegionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_InfoUsuario",
                columns: table => new
                {
                    HER_InfoUsuarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_NombreCompleto = table.Column<string>(nullable: true),
                    HER_Correo = table.Column<string>(nullable: true),
                    HER_UserName = table.Column<string>(nullable: true),
                    HER_Direccion = table.Column<string>(nullable: true),
                    HER_Telefono = table.Column<string>(nullable: true),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_FechaActualizacion = table.Column<DateTime>(nullable: false),
                    HER_Activo = table.Column<bool>(nullable: false),
                    HER_EstaEnReasignacion = table.Column<bool>(nullable: false),
                    HER_EstaEnBajaDefinitiva = table.Column<bool>(nullable: false),
                    HER_Titular = table.Column<string>(nullable: true),
                    HER_RolNombre = table.Column<string>(nullable: true),
                    HER_Puesto = table.Column<string>(nullable: true),
                    HER_EsUnico = table.Column<bool>(nullable: false),
                    HER_BandejaUsuario = table.Column<string>(nullable: true),
                    HER_BandejaPermiso = table.Column<int>(nullable: false),
                    HER_BandejaNombre = table.Column<string>(nullable: true),
                    HER_PermisoAA = table.Column<bool>(nullable: false),
                    HER_AreaId = table.Column<int>(nullable: false),
                    HER_UsuarioId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_InfoUsuario", x => x.HER_InfoUsuarioId);
                    table.ForeignKey(
                        name: "FK_HER_InfoUsuario_HER_Area_HER_AreaId",
                        column: x => x.HER_AreaId,
                        principalTable: "HER_Area",
                        principalColumn: "HER_AreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_InfoUsuario_AspNetUsers_HER_UsuarioId",
                        column: x => x.HER_UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Recopilacion",
                columns: table => new
                {
                    HER_RecopilacionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Usuarios = table.Column<long>(nullable: false),
                    DocumentosEnviados = table.Column<long>(nullable: false),
                    DocumentosRecibidos = table.Column<long>(nullable: false),
                    HER_AreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Recopilacion", x => x.HER_RecopilacionId);
                    table.ForeignKey(
                        name: "FK_HER_Recopilacion_HER_Area_HER_AreaId",
                        column: x => x.HER_AreaId,
                        principalTable: "HER_Area",
                        principalColumn: "HER_AreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Carpeta",
                columns: table => new
                {
                    HER_CarpetaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_FechaActualizacion = table.Column<DateTime>(nullable: false),
                    HER_Nivel = table.Column<int>(nullable: false),
                    HER_CarpetaPadreId = table.Column<int>(nullable: true),
                    HER_InfoUsuarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Carpeta", x => x.HER_CarpetaId);
                    table.ForeignKey(
                        name: "FK_HER_Carpeta_HER_Carpeta_HER_CarpetaPadreId",
                        column: x => x.HER_CarpetaPadreId,
                        principalTable: "HER_Carpeta",
                        principalColumn: "HER_CarpetaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Carpeta_HER_InfoUsuario_HER_InfoUsuarioId",
                        column: x => x.HER_InfoUsuarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Categoria",
                columns: table => new
                {
                    HER_CategoriaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_Tipo = table.Column<int>(nullable: false),
                    HER_InfoUsuarioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Categoria", x => x.HER_CategoriaId);
                    table.ForeignKey(
                        name: "FK_HER_Categoria_HER_InfoUsuario_HER_InfoUsuarioId",
                        column: x => x.HER_InfoUsuarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Delegar",
                columns: table => new
                {
                    HER_DelegarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_FechaActualizacion = table.Column<DateTime>(nullable: false),
                    HER_Tipo = table.Column<int>(nullable: false),
                    HER_TitularId = table.Column<int>(nullable: false),
                    HER_DelegadoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Delegar", x => x.HER_DelegarId);
                    table.ForeignKey(
                        name: "FK_HER_Delegar_HER_InfoUsuario_HER_DelegadoId",
                        column: x => x.HER_DelegadoId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Delegar_HER_InfoUsuario_HER_TitularId",
                        column: x => x.HER_TitularId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Documento",
                columns: table => new
                {
                    HER_DocumentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Folio = table.Column<string>(nullable: true),
                    HER_Asunto = table.Column<string>(nullable: true),
                    HER_NoInterno = table.Column<string>(nullable: true),
                    HER_Cuerpo = table.Column<string>(nullable: true),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_Firma = table.Column<string>(nullable: true),
                    HER_TipoId = table.Column<int>(nullable: false),
                    HER_DocumentoTitularId = table.Column<int>(nullable: false),
                    HER_DocumentoCreadorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Documento", x => x.HER_DocumentoId);
                    table.ForeignKey(
                        name: "FK_HER_Documento_HER_InfoUsuario_HER_DocumentoCreadorId",
                        column: x => x.HER_DocumentoCreadorId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Documento_HER_InfoUsuario_HER_DocumentoTitularId",
                        column: x => x.HER_DocumentoTitularId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Documento_HER_TipoDocumento_HER_TipoId",
                        column: x => x.HER_TipoId,
                        principalTable: "HER_TipoDocumento",
                        principalColumn: "HER_TipoDocumentoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_DocumentoBase",
                columns: table => new
                {
                    HER_DocumentoBaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Folio = table.Column<string>(nullable: true),
                    HER_RequiereRespuesta = table.Column<bool>(nullable: false),
                    HER_EnRevision = table.Column<bool>(nullable: false),
                    HER_Asunto = table.Column<string>(nullable: true),
                    HER_NoInterno = table.Column<string>(nullable: true),
                    HER_Cuerpo = table.Column<string>(nullable: true),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_FechaPropuesta = table.Column<DateTime>(nullable: false),
                    HER_ImportanciaId = table.Column<int>(nullable: false),
                    HER_TipoId = table.Column<int>(nullable: false),
                    HER_EstadoId = table.Column<int>(nullable: false),
                    HER_VisibilidadId = table.Column<int>(nullable: false),
                    HER_DocumentoBaseTitularId = table.Column<int>(nullable: false),
                    HER_DocumentoBaseCreadorId = table.Column<int>(nullable: false),
                    HER_AnexoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_DocumentoBase", x => x.HER_DocumentoBaseId);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_Anexo_HER_AnexoId",
                        column: x => x.HER_AnexoId,
                        principalTable: "HER_Anexo",
                        principalColumn: "HER_AnexoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_InfoUsuario_HER_DocumentoBaseCreadorId",
                        column: x => x.HER_DocumentoBaseCreadorId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_InfoUsuario_HER_DocumentoBaseTitularId",
                        column: x => x.HER_DocumentoBaseTitularId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_EstadoDocumento_HER_EstadoId",
                        column: x => x.HER_EstadoId,
                        principalTable: "HER_EstadoDocumento",
                        principalColumn: "HER_EstadoDocumentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_Importancia_HER_ImportanciaId",
                        column: x => x.HER_ImportanciaId,
                        principalTable: "HER_Importancia",
                        principalColumn: "HER_ImportanciaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_TipoDocumento_HER_TipoId",
                        column: x => x.HER_TipoId,
                        principalTable: "HER_TipoDocumento",
                        principalColumn: "HER_TipoDocumentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_DocumentoBase_HER_Visibilidad_HER_VisibilidadId",
                        column: x => x.HER_VisibilidadId,
                        principalTable: "HER_Visibilidad",
                        principalColumn: "HER_VisibilidadId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Grupo",
                columns: table => new
                {
                    HER_GrupoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_CreadorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Grupo", x => x.HER_GrupoId);
                    table.ForeignKey(
                        name: "FK_HER_Grupo_HER_InfoUsuario_HER_CreadorId",
                        column: x => x.HER_CreadorId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Plantilla",
                columns: table => new
                {
                    HER_PlantillaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_Texto = table.Column<string>(nullable: true),
                    HER_Fecha_Registro = table.Column<DateTime>(nullable: false),
                    HER_InfoUsuarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Plantilla", x => x.HER_PlantillaId);
                    table.ForeignKey(
                        name: "FK_HER_Plantilla_HER_InfoUsuario_HER_InfoUsuarioId",
                        column: x => x.HER_InfoUsuarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_ServicioIntegrante",
                columns: table => new
                {
                    HER_ServicioIntegranteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_ServicioId = table.Column<int>(nullable: false),
                    HER_IntegranteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_ServicioIntegrante", x => x.HER_ServicioIntegranteId);
                    table.ForeignKey(
                        name: "FK_HER_ServicioIntegrante_HER_InfoUsuario_HER_IntegranteId",
                        column: x => x.HER_IntegranteId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_ServicioIntegrante_HER_Servicio_HER_ServicioId",
                        column: x => x.HER_ServicioId,
                        principalTable: "HER_Servicio",
                        principalColumn: "HER_ServicioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Tramite",
                columns: table => new
                {
                    HER_TramiteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Nombre = table.Column<string>(nullable: true),
                    HER_Descripcion = table.Column<string>(nullable: true),
                    HER_Dias = table.Column<int>(nullable: false),
                    HER_Estado = table.Column<int>(nullable: false),
                    HER_FechaRegistro = table.Column<DateTime>(nullable: false),
                    HER_FechaActualizacion = table.Column<DateTime>(nullable: false),
                    HER_CreadorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Tramite", x => x.HER_TramiteId);
                    table.ForeignKey(
                        name: "FK_HER_Tramite_HER_InfoUsuario_HER_CreadorId",
                        column: x => x.HER_CreadorId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_CategoriaDocumento",
                columns: table => new
                {
                    HER_CategoriaDocumentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_CategoriaId = table.Column<int>(nullable: false),
                    HER_DocumentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_CategoriaDocumento", x => x.HER_CategoriaDocumentoId);
                    table.ForeignKey(
                        name: "FK_HER_CategoriaDocumento_HER_Categoria_HER_CategoriaId",
                        column: x => x.HER_CategoriaId,
                        principalTable: "HER_Categoria",
                        principalColumn: "HER_CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_CategoriaDocumento_HER_Documento_HER_DocumentoId",
                        column: x => x.HER_DocumentoId,
                        principalTable: "HER_Documento",
                        principalColumn: "HER_DocumentoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_CategoriaDocumentoBase",
                columns: table => new
                {
                    HER_CategoriaDocumentoBaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_CategoriaId = table.Column<int>(nullable: false),
                    HER_DocumentoBaseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_CategoriaDocumentoBase", x => x.HER_CategoriaDocumentoBaseId);
                    table.ForeignKey(
                        name: "FK_HER_CategoriaDocumentoBase_HER_Categoria_HER_CategoriaId",
                        column: x => x.HER_CategoriaId,
                        principalTable: "HER_Categoria",
                        principalColumn: "HER_CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_CategoriaDocumentoBase_HER_DocumentoBase_HER_DocumentoBaseId",
                        column: x => x.HER_DocumentoBaseId,
                        principalTable: "HER_DocumentoBase",
                        principalColumn: "HER_DocumentoBaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Destinatario",
                columns: table => new
                {
                    HER_DestinatarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Tipo = table.Column<int>(nullable: false),
                    HER_UDestinatarioId = table.Column<int>(nullable: false),
                    HER_DocumentoBaseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Destinatario", x => x.HER_DestinatarioId);
                    table.ForeignKey(
                        name: "FK_HER_Destinatario_HER_DocumentoBase_HER_DocumentoBaseId",
                        column: x => x.HER_DocumentoBaseId,
                        principalTable: "HER_DocumentoBase",
                        principalColumn: "HER_DocumentoBaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Destinatario_HER_InfoUsuario_HER_UDestinatarioId",
                        column: x => x.HER_UDestinatarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_EnvioRevision",
                columns: table => new
                {
                    HER_EnvioRevisionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_EstadoRemitente = table.Column<int>(nullable: false),
                    HER_EstadoDestinatario = table.Column<int>(nullable: false),
                    HER_Fecha = table.Column<DateTime>(nullable: false),
                    HER_DocumentoBaseId = table.Column<int>(nullable: false),
                    HER_RevisionRemitenteId = table.Column<int>(nullable: false),
                    HER_RevisionDestinatarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_EnvioRevision", x => x.HER_EnvioRevisionId);
                    table.ForeignKey(
                        name: "FK_HER_EnvioRevision_HER_DocumentoBase_HER_DocumentoBaseId",
                        column: x => x.HER_DocumentoBaseId,
                        principalTable: "HER_DocumentoBase",
                        principalColumn: "HER_DocumentoBaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_EnvioRevision_HER_InfoUsuario_HER_RevisionDestinatarioId",
                        column: x => x.HER_RevisionDestinatarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_EnvioRevision_HER_InfoUsuario_HER_RevisionRemitenteId",
                        column: x => x.HER_RevisionRemitenteId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_GrupoIntegrante",
                columns: table => new
                {
                    HER_GrupoIntegranteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_GrupoId = table.Column<int>(nullable: false),
                    HER_IntegranteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_GrupoIntegrante", x => x.HER_GrupoIntegranteId);
                    table.ForeignKey(
                        name: "FK_HER_GrupoIntegrante_HER_Grupo_HER_GrupoId",
                        column: x => x.HER_GrupoId,
                        principalTable: "HER_Grupo",
                        principalColumn: "HER_GrupoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_GrupoIntegrante_HER_InfoUsuario_HER_IntegranteId",
                        column: x => x.HER_IntegranteId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Envio",
                columns: table => new
                {
                    HER_EnvioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_DeDireccion = table.Column<string>(nullable: true),
                    HER_DeTelefono = table.Column<string>(nullable: true),
                    HER_UsuarioOrigenId = table.Column<int>(nullable: true),
                    HER_Orden = table.Column<int>(nullable: false),
                    HER_TotalPara = table.Column<int>(nullable: false),
                    HER_TotalCCP = table.Column<int>(nullable: false),
                    HER_TotalParaRespuestas = table.Column<int>(nullable: false),
                    HER_TotalCCPRespuestas = table.Column<int>(nullable: false),
                    HER_Indicaciones = table.Column<string>(nullable: true),
                    HER_RequiereRespuesta = table.Column<bool>(nullable: false),
                    HER_FechaEnvio = table.Column<DateTime>(nullable: false),
                    HER_FechaPropuesta = table.Column<DateTime>(nullable: false),
                    HER_GrupoEnvio = table.Column<int>(nullable: false),
                    HER_EsReenvio = table.Column<bool>(nullable: false),
                    HER_DocumentoId = table.Column<int>(nullable: false),
                    HER_DeId = table.Column<int>(nullable: false),
                    HER_UsuarioEnviaId = table.Column<int>(nullable: false),
                    HER_TipoEnvioId = table.Column<int>(nullable: false),
                    HER_EnvioPadreId = table.Column<int>(nullable: true),
                    HER_EstadoEnvioId = table.Column<int>(nullable: false),
                    HER_VisibilidadId = table.Column<int>(nullable: false),
                    HER_ImportanciaId = table.Column<int>(nullable: false),
                    HER_AnexoId = table.Column<int>(nullable: true),
                    HER_CarpetaId = table.Column<int>(nullable: true),
                    HER_TramiteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Envio", x => x.HER_EnvioId);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Anexo_HER_AnexoId",
                        column: x => x.HER_AnexoId,
                        principalTable: "HER_Anexo",
                        principalColumn: "HER_AnexoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Carpeta_HER_CarpetaId",
                        column: x => x.HER_CarpetaId,
                        principalTable: "HER_Carpeta",
                        principalColumn: "HER_CarpetaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_InfoUsuario_HER_DeId",
                        column: x => x.HER_DeId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Documento_HER_DocumentoId",
                        column: x => x.HER_DocumentoId,
                        principalTable: "HER_Documento",
                        principalColumn: "HER_DocumentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Envio_HER_EnvioPadreId",
                        column: x => x.HER_EnvioPadreId,
                        principalTable: "HER_Envio",
                        principalColumn: "HER_EnvioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_EstadoEnvio_HER_EstadoEnvioId",
                        column: x => x.HER_EstadoEnvioId,
                        principalTable: "HER_EstadoEnvio",
                        principalColumn: "HER_EstadoEnvioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Importancia_HER_ImportanciaId",
                        column: x => x.HER_ImportanciaId,
                        principalTable: "HER_Importancia",
                        principalColumn: "HER_ImportanciaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_TipoEnvio_HER_TipoEnvioId",
                        column: x => x.HER_TipoEnvioId,
                        principalTable: "HER_TipoEnvio",
                        principalColumn: "HER_TipoEnvioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Tramite_HER_TramiteId",
                        column: x => x.HER_TramiteId,
                        principalTable: "HER_Tramite",
                        principalColumn: "HER_TramiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_InfoUsuario_HER_UsuarioEnviaId",
                        column: x => x.HER_UsuarioEnviaId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_InfoUsuario_HER_UsuarioOrigenId",
                        column: x => x.HER_UsuarioOrigenId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Envio_HER_Visibilidad_HER_VisibilidadId",
                        column: x => x.HER_VisibilidadId,
                        principalTable: "HER_Visibilidad",
                        principalColumn: "HER_VisibilidadId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Cancelado",
                columns: table => new
                {
                    HER_CanceladoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Fecha = table.Column<DateTime>(nullable: false),
                    HER_Motivo = table.Column<string>(nullable: true),
                    HER_EnvioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Cancelado", x => x.HER_CanceladoId);
                    table.ForeignKey(
                        name: "FK_HER_Cancelado_HER_Envio_HER_EnvioId",
                        column: x => x.HER_EnvioId,
                        principalTable: "HER_Envio",
                        principalColumn: "HER_EnvioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HER_HistorialCorreo",
                columns: table => new
                {
                    HER_HistorialCorreoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Fecha = table.Column<DateTime>(nullable: false),
                    HER_InfoUsuarioId = table.Column<int>(nullable: false),
                    HER_Delegado = table.Column<string>(nullable: true),
                    HER_Destinatario = table.Column<string>(nullable: true),
                    HER_EnvioId = table.Column<int>(nullable: false),
                    HER_Tipo = table.Column<int>(nullable: false),
                    HER_TipoEnvio = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_HistorialCorreo", x => x.HER_HistorialCorreoId);
                    table.ForeignKey(
                        name: "FK_HER_HistorialCorreo_HER_Envio_HER_EnvioId",
                        column: x => x.HER_EnvioId,
                        principalTable: "HER_Envio",
                        principalColumn: "HER_EnvioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HER_HistorialCorreo_HER_InfoUsuario_HER_InfoUsuarioId",
                        column: x => x.HER_InfoUsuarioId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HER_Recepcion",
                columns: table => new
                {
                    HER_RecepcionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_TipoPara = table.Column<int>(nullable: false),
                    HER_TieneRespuesta = table.Column<bool>(nullable: false),
                    HER_EstaLeido = table.Column<bool>(nullable: false),
                    HER_FechaRecepcion = table.Column<DateTime>(nullable: false),
                    HER_EnvioId = table.Column<int>(nullable: false),
                    HER_ParaId = table.Column<int>(nullable: false),
                    HER_CarpetaId = table.Column<int>(nullable: true),
                    HER_EstadoEnvioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Recepcion", x => x.HER_RecepcionId);
                    table.ForeignKey(
                        name: "FK_HER_Recepcion_HER_Carpeta_HER_CarpetaId",
                        column: x => x.HER_CarpetaId,
                        principalTable: "HER_Carpeta",
                        principalColumn: "HER_CarpetaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Recepcion_HER_Envio_HER_EnvioId",
                        column: x => x.HER_EnvioId,
                        principalTable: "HER_Envio",
                        principalColumn: "HER_EnvioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Recepcion_HER_EstadoEnvio_HER_EstadoEnvioId",
                        column: x => x.HER_EstadoEnvioId,
                        principalTable: "HER_EstadoEnvio",
                        principalColumn: "HER_EstadoEnvioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_Recepcion_HER_InfoUsuario_HER_ParaId",
                        column: x => x.HER_ParaId,
                        principalTable: "HER_InfoUsuario",
                        principalColumn: "HER_InfoUsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_Compromiso",
                columns: table => new
                {
                    HER_CompromisoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_Fecha = table.Column<DateTime>(nullable: false),
                    HER_Tipo = table.Column<int>(nullable: false),
                    HER_Estado = table.Column<int>(nullable: false),
                    HER_Registro = table.Column<DateTime>(nullable: false),
                    HER_Motivo = table.Column<string>(nullable: true),
                    HER_RecepcionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_Compromiso", x => x.HER_CompromisoId);
                    table.ForeignKey(
                        name: "FK_HER_Compromiso_HER_Recepcion_HER_RecepcionId",
                        column: x => x.HER_RecepcionId,
                        principalTable: "HER_Recepcion",
                        principalColumn: "HER_RecepcionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HER_RecepcionCategoria",
                columns: table => new
                {
                    HER_RecepcionCategoriaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HER_RecepcionId = table.Column<int>(nullable: false),
                    HER_CategoriaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HER_RecepcionCategoria", x => x.HER_RecepcionCategoriaId);
                    table.ForeignKey(
                        name: "FK_HER_RecepcionCategoria_HER_Categoria_HER_CategoriaId",
                        column: x => x.HER_CategoriaId,
                        principalTable: "HER_Categoria",
                        principalColumn: "HER_CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HER_RecepcionCategoria_HER_Recepcion_HER_RecepcionId",
                        column: x => x.HER_RecepcionId,
                        principalTable: "HER_Recepcion",
                        principalColumn: "HER_RecepcionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HER_AnexoArchivo_HER_AnexoId",
                table: "HER_AnexoArchivo",
                column: "HER_AnexoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_AnexoArchivo_HER_AnexoRutaId",
                table: "HER_AnexoArchivo",
                column: "HER_AnexoRutaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_AnexoArea_HER_AnexoRutaId",
                table: "HER_AnexoArea",
                column: "HER_AnexoRutaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Area_HER_Area_PadreId",
                table: "HER_Area",
                column: "HER_Area_PadreId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Area_HER_LogoId",
                table: "HER_Area",
                column: "HER_LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Area_HER_RegionId",
                table: "HER_Area",
                column: "HER_RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_CalendarioContenido_HER_CalendarioId",
                table: "HER_CalendarioContenido",
                column: "HER_CalendarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Cancelado_HER_EnvioId",
                table: "HER_Cancelado",
                column: "HER_EnvioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HER_Carpeta_HER_CarpetaPadreId",
                table: "HER_Carpeta",
                column: "HER_CarpetaPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Carpeta_HER_InfoUsuarioId",
                table: "HER_Carpeta",
                column: "HER_InfoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Categoria_HER_InfoUsuarioId",
                table: "HER_Categoria",
                column: "HER_InfoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_CategoriaDocumento_HER_CategoriaId",
                table: "HER_CategoriaDocumento",
                column: "HER_CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_CategoriaDocumento_HER_DocumentoId",
                table: "HER_CategoriaDocumento",
                column: "HER_DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_CategoriaDocumentoBase_HER_CategoriaId",
                table: "HER_CategoriaDocumentoBase",
                column: "HER_CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_CategoriaDocumentoBase_HER_DocumentoBaseId",
                table: "HER_CategoriaDocumentoBase",
                column: "HER_DocumentoBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Compromiso_HER_RecepcionId",
                table: "HER_Compromiso",
                column: "HER_RecepcionId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_ConfiguracionUsuario_HER_ColorId",
                table: "HER_ConfiguracionUsuario",
                column: "HER_ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_ConfiguracionUsuario_HER_UsuarioId",
                table: "HER_ConfiguracionUsuario",
                column: "HER_UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Delegar_HER_DelegadoId",
                table: "HER_Delegar",
                column: "HER_DelegadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Delegar_HER_TitularId",
                table: "HER_Delegar",
                column: "HER_TitularId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Destinatario_HER_DocumentoBaseId",
                table: "HER_Destinatario",
                column: "HER_DocumentoBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Destinatario_HER_UDestinatarioId",
                table: "HER_Destinatario",
                column: "HER_UDestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Documento_HER_DocumentoCreadorId",
                table: "HER_Documento",
                column: "HER_DocumentoCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Documento_HER_DocumentoTitularId",
                table: "HER_Documento",
                column: "HER_DocumentoTitularId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Documento_HER_TipoId",
                table: "HER_Documento",
                column: "HER_TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_AnexoId",
                table: "HER_DocumentoBase",
                column: "HER_AnexoId",
                unique: true,
                filter: "[HER_AnexoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_DocumentoBaseCreadorId",
                table: "HER_DocumentoBase",
                column: "HER_DocumentoBaseCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_DocumentoBaseTitularId",
                table: "HER_DocumentoBase",
                column: "HER_DocumentoBaseTitularId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_EstadoId",
                table: "HER_DocumentoBase",
                column: "HER_EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_ImportanciaId",
                table: "HER_DocumentoBase",
                column: "HER_ImportanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_TipoId",
                table: "HER_DocumentoBase",
                column: "HER_TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_DocumentoBase_HER_VisibilidadId",
                table: "HER_DocumentoBase",
                column: "HER_VisibilidadId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_AnexoId",
                table: "HER_Envio",
                column: "HER_AnexoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_CarpetaId",
                table: "HER_Envio",
                column: "HER_CarpetaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_DeId",
                table: "HER_Envio",
                column: "HER_DeId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_DocumentoId",
                table: "HER_Envio",
                column: "HER_DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_EnvioPadreId",
                table: "HER_Envio",
                column: "HER_EnvioPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_EstadoEnvioId",
                table: "HER_Envio",
                column: "HER_EstadoEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_ImportanciaId",
                table: "HER_Envio",
                column: "HER_ImportanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_TipoEnvioId",
                table: "HER_Envio",
                column: "HER_TipoEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_TramiteId",
                table: "HER_Envio",
                column: "HER_TramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_UsuarioEnviaId",
                table: "HER_Envio",
                column: "HER_UsuarioEnviaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_UsuarioOrigenId",
                table: "HER_Envio",
                column: "HER_UsuarioOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Envio_HER_VisibilidadId",
                table: "HER_Envio",
                column: "HER_VisibilidadId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_EnvioRevision_HER_DocumentoBaseId",
                table: "HER_EnvioRevision",
                column: "HER_DocumentoBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_EnvioRevision_HER_RevisionDestinatarioId",
                table: "HER_EnvioRevision",
                column: "HER_RevisionDestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_EnvioRevision_HER_RevisionRemitenteId",
                table: "HER_EnvioRevision",
                column: "HER_RevisionRemitenteId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Grupo_HER_CreadorId",
                table: "HER_Grupo",
                column: "HER_CreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_GrupoIntegrante_HER_GrupoId",
                table: "HER_GrupoIntegrante",
                column: "HER_GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_GrupoIntegrante_HER_IntegranteId",
                table: "HER_GrupoIntegrante",
                column: "HER_IntegranteId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_HistorialCorreo_HER_EnvioId",
                table: "HER_HistorialCorreo",
                column: "HER_EnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_HistorialCorreo_HER_InfoUsuarioId",
                table: "HER_HistorialCorreo",
                column: "HER_InfoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_InfoUsuario_HER_AreaId",
                table: "HER_InfoUsuario",
                column: "HER_AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_InfoUsuario_HER_UsuarioId",
                table: "HER_InfoUsuario",
                column: "HER_UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Plantilla_HER_InfoUsuarioId",
                table: "HER_Plantilla",
                column: "HER_InfoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Recepcion_HER_CarpetaId",
                table: "HER_Recepcion",
                column: "HER_CarpetaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Recepcion_HER_EnvioId",
                table: "HER_Recepcion",
                column: "HER_EnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Recepcion_HER_EstadoEnvioId",
                table: "HER_Recepcion",
                column: "HER_EstadoEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Recepcion_HER_ParaId",
                table: "HER_Recepcion",
                column: "HER_ParaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_RecepcionCategoria_HER_CategoriaId",
                table: "HER_RecepcionCategoria",
                column: "HER_CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_RecepcionCategoria_HER_RecepcionId",
                table: "HER_RecepcionCategoria",
                column: "HER_RecepcionId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Recopilacion_HER_AreaId",
                table: "HER_Recopilacion",
                column: "HER_AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Servicio_HER_CreadorId",
                table: "HER_Servicio",
                column: "HER_CreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Servicio_HER_RegionId",
                table: "HER_Servicio",
                column: "HER_RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_ServicioIntegrante_HER_IntegranteId",
                table: "HER_ServicioIntegrante",
                column: "HER_IntegranteId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_ServicioIntegrante_HER_ServicioId",
                table: "HER_ServicioIntegrante",
                column: "HER_ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_HER_Tramite_HER_CreadorId",
                table: "HER_Tramite",
                column: "HER_CreadorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HER_AnexoArchivo");

            migrationBuilder.DropTable(
                name: "HER_AnexoGeneral");

            migrationBuilder.DropTable(
                name: "HER_CalendarioContenido");

            migrationBuilder.DropTable(
                name: "HER_Cancelado");

            migrationBuilder.DropTable(
                name: "HER_CategoriaDocumento");

            migrationBuilder.DropTable(
                name: "HER_CategoriaDocumentoBase");

            migrationBuilder.DropTable(
                name: "HER_Compromiso");

            migrationBuilder.DropTable(
                name: "HER_Configuracion");

            migrationBuilder.DropTable(
                name: "HER_ConfiguracionUsuario");

            migrationBuilder.DropTable(
                name: "HER_Delegar");

            migrationBuilder.DropTable(
                name: "HER_Destinatario");

            migrationBuilder.DropTable(
                name: "HER_EnvioRevision");

            migrationBuilder.DropTable(
                name: "HER_Extension");

            migrationBuilder.DropTable(
                name: "HER_GrupoIntegrante");

            migrationBuilder.DropTable(
                name: "HER_HistorialCorreo");

            migrationBuilder.DropTable(
                name: "HER_Plantilla");

            migrationBuilder.DropTable(
                name: "HER_RecepcionCategoria");

            migrationBuilder.DropTable(
                name: "HER_Recopilacion");

            migrationBuilder.DropTable(
                name: "HER_Rol");

            migrationBuilder.DropTable(
                name: "HER_ServicioIntegrante");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "HER_Calendario");

            migrationBuilder.DropTable(
                name: "HER_Color");

            migrationBuilder.DropTable(
                name: "HER_DocumentoBase");

            migrationBuilder.DropTable(
                name: "HER_Grupo");

            migrationBuilder.DropTable(
                name: "HER_Categoria");

            migrationBuilder.DropTable(
                name: "HER_Recepcion");

            migrationBuilder.DropTable(
                name: "HER_Servicio");

            migrationBuilder.DropTable(
                name: "HER_EstadoDocumento");

            migrationBuilder.DropTable(
                name: "HER_Envio");

            migrationBuilder.DropTable(
                name: "HER_Anexo");

            migrationBuilder.DropTable(
                name: "HER_Carpeta");

            migrationBuilder.DropTable(
                name: "HER_Documento");

            migrationBuilder.DropTable(
                name: "HER_EstadoEnvio");

            migrationBuilder.DropTable(
                name: "HER_Importancia");

            migrationBuilder.DropTable(
                name: "HER_TipoEnvio");

            migrationBuilder.DropTable(
                name: "HER_Tramite");

            migrationBuilder.DropTable(
                name: "HER_Visibilidad");

            migrationBuilder.DropTable(
                name: "HER_TipoDocumento");

            migrationBuilder.DropTable(
                name: "HER_InfoUsuario");

            migrationBuilder.DropTable(
                name: "HER_Area");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "HER_AnexoArea");

            migrationBuilder.DropTable(
                name: "HER_Region");

            migrationBuilder.DropTable(
                name: "HER_AnexoRuta");
        }
    }
}
