using System;
using System.Collections.Generic;
using System.Text;
using Hermes2018.Models;
using Hermes2018.Models.Grupo;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Rol;
using Hermes2018.Models.Area;
using Hermes2018.Models.Plantilla;
using Hermes2018.Models.Configuracion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Hermes2018.Models.Calendario;
using Hermes2018.Models.Carpeta;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Servicio;
using Hermes2018.Models.Categoria;
using Hermes2018.Models.Recopilacion;
using Hermes2018.Models.Tramite;
using Hermes2018.Models.Constancia;
using Hermes2018.Models.Bitacora;

namespace Hermes2018.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HER_Usuario> HER_Usuario { get; set; }
        public DbSet<HER_InfoUsuario> HER_InfoUsuario { get; set; }
        public DbSet<HER_Region> HER_Region { get; set; }
        public DbSet<HER_Area> HER_Area { get; set; }
        public DbSet<HER_Rol> HER_Rol { get; set; }
        public DbSet<HER_Delegar> HER_Delegar { get; set; } 
        public DbSet<HER_Documento> HER_Documento { get; set; }
        public DbSet<HER_Destinatario> HER_Destinatario { get; set; }
        public DbSet<HER_TipoDocumento> HER_TipoDocumento { get; set; }
        public DbSet<HER_TipoEnvio> HER_TipoEnvio { get; set; }
        public DbSet<HER_Importancia> HER_Importancia { get; set; }
        public DbSet<HER_EstadoDocumento> HER_EstadoDocumento { get; set; }
        public DbSet<HER_EstadoEnvio> HER_EstadoEnvio { get; set; }
        public DbSet<HER_Categoria> HER_Categoria { get; set; }
        public DbSet<HER_CategoriaDocumento> HER_CategoriaDocumento { get; set; }
        public DbSet<HER_Anexo> HER_Anexo { get; set; }
        public DbSet<HER_AnexoArchivo> HER_AnexoArchivo { get; set; }
        public DbSet<HER_Servicio> HER_Servicio { get; set; }
        public DbSet<HER_ServicioIntegrante> HER_ServicioIntegrante { get; set; }
        public DbSet<HER_Grupo> HER_Grupo { get; set; }
        public DbSet<HER_GrupoIntegrante> HER_GrupoIntegrante { get; set; }
        public DbSet<HER_EnvioRevision> HER_EnvioRevision { get; set; }
        public DbSet<HER_Plantilla> HER_Plantilla { get; set; }
        public DbSet<HER_Color> HER_Color { get; set; }
        public DbSet<HER_Configuracion> HER_Configuracion { get; set; }
        //public DbSet<HER_ConfiguracionColeccion> HER_ConfiguracionColeccion { get; set; }
        public DbSet<HER_AnexoArea> HER_AnexoArea { get; set; }
        public DbSet<HER_AnexoGeneral> HER_AnexoGeneral { get; set; }
        public DbSet<HER_AnexoRuta> HER_AnexoRuta { get; set; }
        public DbSet<HER_ConfiguracionUsuario> HER_ConfiguracionUsuario { get; set; }
        public DbSet<HER_Carpeta> HER_Carpeta { get; set; }
        public DbSet<HER_Envio> HER_Envio { get; set; }
        public DbSet<HER_Recepcion> HER_Recepcion { get; set; }
        public DbSet<HER_RecepcionCategoria> HER_RecepcionCategoria  { get; set; }
        public DbSet<HER_Calendario> HER_Calendario { get; set; }
        public DbSet<HER_CalendarioContenido> HER_CalendarioContenido { get; set; }
        public DbSet<HER_Extension> HER_Extension { get; set; }
        public DbSet<HER_Visibilidad> HER_Visibilidad { get; set; }
        public DbSet<HER_DocumentoBase> HER_DocumentoBase { get; set; }
        public DbSet<HER_CategoriaDocumentoBase> HER_CategoriaDocumentoBase { get; set; }
        public DbSet<HER_Compromiso> HER_Compromiso { get; set; } 
        public DbSet<HER_HistorialCorreo> HER_HistorialCorreo { get; set; }
        public DbSet<HER_Recopilacion> HER_Recopilacion { get; set; }
        public DbSet<HER_Tramite> HER_Tramite { get; set; }
        //Empieza Modulo de constancias
        //public DbSet<HER_ConstanciaGeneral> HER_Constancia { get; set; }
        //public DbSet<HER_TipoConstancia> HER_TipoConstancia { get; set; }
        //public DbSet<HER_TipoPersonalConstancia> HER_TipoPersonalConstancias { get; set; }

        public DbSet<HER_Bitacora> HER_Bitacora { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HER_Area>()
                .HasOne(x => x.HER_Region)
                .WithMany(x => x.HER_Areas)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_AnexoArchivo>()
                .HasOne(x => x.HER_Anexo)
                .WithMany(x => x.HER_AnexoArchivos)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_AnexoArchivo>()
                .HasIndex(b => b.HER_AnexoRutaId)
                .IsUnique(false);

            builder.Entity<HER_AnexoArea>()
                .HasIndex(b => b.HER_AnexoRutaId)
                .IsUnique(false);

            builder.Entity<HER_CalendarioContenido>()
               .HasOne(ug => ug.HER_Calendario)
               .WithMany(g => g.HER_Contenido)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_CalendarioContenido>()
            .HasIndex(b => b.HER_CalendarioId)
            .IsUnique(false);

            builder.Entity<HER_RecepcionCategoria>()
                .HasOne(a => a.HER_Recepcion)
                .WithMany(g => g.HER_Categorias)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_RecepcionCategoria>()
                .HasOne(ug => ug.HER_Categoria)
                .WithMany(g => g.HER_Recepciones)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_RecepcionCategoria>()
                .HasIndex(b => b.HER_RecepcionId)
                .IsUnique(false);

            builder.Entity<HER_RecepcionCategoria>()
                .HasIndex(b => b.HER_CategoriaId)
                .IsUnique(false);

            builder.Entity<HER_Envio>()
                .HasOne(x => x.HER_Documento)
                .WithOne(y => y.HER_Envio)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
                .HasOne(x => x.HER_De)
                .WithOne(y => y.HER_EnvioUsuarioDe)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
                .HasOne(x => x.HER_UsuarioEnvia)
                .WithOne(y => y.HER_EnvioUsuarioEnvia)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
             .HasOne(x => x.HER_TipoEnvio)
             .WithMany(y => y.HER_Envios)
             .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
              .HasOne(x => x.HER_EstadoEnvio)
              .WithMany(y => y.HER_Envios)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
              .HasOne(x => x.HER_Visibilidad)
              .WithMany(y => y.HER_Envios)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
               .HasOne(p => p.HER_Importancia)
               .WithMany(b => b.HER_Envios)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_DocumentoId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_DeId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_UsuarioEnviaId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_TipoEnvioId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_EstadoEnvioId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_VisibilidadId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_ImportanciaId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_AnexoId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_CarpetaId)
               .IsUnique(false);

            builder.Entity<HER_Envio>()
               .HasIndex(b => b.HER_TramiteId)
               .IsUnique(false);

            builder.Entity<HER_Recepcion>()
               .HasOne(x => x.HER_Envio)
               .WithMany(y => y.HER_Recepciones)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Recepcion>()
                .HasOne(x => x.HER_Para)
                .WithOne(y => y.HER_RecepcionUsuarioPara)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Recepcion>()
              .HasOne(x => x.HER_EstadoEnvio)
              .WithMany(y => y.HER_Recepciones)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Recepcion>()
                .HasIndex(b => b.HER_EnvioId)
                .IsUnique(false);

            builder.Entity<HER_Recepcion>()
                .HasIndex(b => b.HER_ParaId)
                .IsUnique(false);

            builder.Entity<HER_Recepcion>()
               .HasIndex(b => b.HER_EstadoEnvioId)
               .IsUnique(false);

            builder.Entity<HER_Plantilla>()
               .HasOne(x => x.HER_InfoUsuario)
               .WithMany(y => y.HER_Plantillas)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Plantilla>()
               .HasIndex(b => b.HER_InfoUsuarioId)
               .IsUnique(false);

            builder.Entity<HER_Carpeta>()
               .HasOne(x => x.HER_InfoUsuario)
               .WithMany(y => y.HER_Carpetas)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_EnvioRevision>()
                .HasOne(ug => ug.HER_DocumentoBase)
                .WithOne(g => g.HER_Revision)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_EnvioRevision>()
                .HasOne(ug => ug.HER_RevisionRemitente)
                .WithOne(g => g.HER_RevRemitente)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_EnvioRevision>()
                .HasOne(ug => ug.HER_RevisionDestinatario)
                .WithOne(g => g.HER_RevDestinatario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_EnvioRevision>()
                    .HasIndex(b => b.HER_DocumentoBaseId)
                    .IsUnique(false);

            builder.Entity<HER_EnvioRevision>()
                    .HasIndex(b => b.HER_RevisionRemitenteId)
                    .IsUnique(false);

            builder.Entity<HER_EnvioRevision>()
                .HasIndex(b => b.HER_RevisionDestinatarioId)
                .IsUnique(false);

            builder.Entity<HER_Categoria>()
                .HasOne(ug => ug.HER_InfoUsuario)
                .WithMany(g => g.HER_Categorias)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Grupo>()
                .HasOne(ug => ug.HER_Creador)
                .WithMany(g => g.HER_Grupos)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_GrupoIntegrante>()
                .HasOne(ug => ug.HER_Grupo)
                .WithMany(g => g.HER_Integrantes)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_GrupoIntegrante>()
                .HasOne(ug => ug.HER_Integrante)
                .WithMany(u => u.HER_GrupoIntegrantes)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Servicio>()
                .HasOne(ug => ug.HER_Creador)
                .WithMany(g => g.HER_Servicios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Servicio>()
                .HasOne(ug => ug.HER_Region)
                .WithMany(g => g.HER_Servicios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_ServicioIntegrante>()
                .HasOne(ug => ug.HER_Servicio)
                .WithMany(g => g.HER_Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_ServicioIntegrante>()
                .HasOne(ug => ug.HER_Integrante)
                .WithMany(u => u.HER_ServicioIntegrantes)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Delegar>()
                .HasOne(p => p.HER_Titular)
                .WithMany(b => b.HER_Delegados)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Delegar>()
                .HasOne(p => p.HER_Delegado)
                .WithMany(b => b.HER_Titulares)
                .OnDelete(DeleteBehavior.ClientSetNull);
            //--
            builder.Entity<HER_DocumentoBase>()
               .HasOne(p => p.HER_Importancia)
               .WithMany(b => b.HER_DocumentosBase)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_DocumentoBase>()
              .HasOne(p => p.HER_Tipo)
              .WithMany(b => b.HER_DocumentosBase)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_DocumentoBase>()
               .HasOne(p => p.HER_Estado)
               .WithMany(b => b.HER_DocumentosBase)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_DocumentoBase>()
               .HasOne(p => p.HER_Visibilidad)
               .WithMany(b => b.HER_DocumentosBase)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_DocumentoBase>()
                .HasOne(p => p.HER_DocumentoBaseTitular)
                .WithMany(b => b.HER_DocBaseTitular)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_DocumentoBase>()
                .HasOne(p => p.HER_DocumentoBaseCreador)
                .WithMany(b => b.HER_DocBaseCreador)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_DocumentoBase>()
               .HasIndex(b => b.HER_ImportanciaId)
               .IsUnique(false);

            builder.Entity<HER_DocumentoBase>()
               .HasIndex(b => b.HER_TipoId)
               .IsUnique(false);

            builder.Entity<HER_DocumentoBase>()
               .HasIndex(b => b.HER_EstadoId)
               .IsUnique(false);

            builder.Entity<HER_DocumentoBase>()
               .HasIndex(b => b.HER_VisibilidadId)
               .IsUnique(false);

            builder.Entity<HER_DocumentoBase>()
               .HasIndex(b => b.HER_DocumentoBaseTitularId)
               .IsUnique(false);

            builder.Entity<HER_DocumentoBase>()
              .HasIndex(b => b.HER_DocumentoBaseCreadorId)
              .IsUnique(false);

            builder.Entity<HER_CategoriaDocumentoBase>()
               .HasOne(ug => ug.HER_Categoria)
               .WithMany(g => g.HER_DocumentosBase)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_CategoriaDocumentoBase>()
               .HasOne(ug => ug.HER_DocumentoBase)
               .WithMany(g => g.HER_Categorias)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_CategoriaDocumentoBase>()
               .HasIndex(b => b.HER_CategoriaId)
               .IsUnique(false);

            builder.Entity<HER_CategoriaDocumentoBase>()
                   .HasIndex(b => b.HER_DocumentoBaseId)
                   .IsUnique(false);
            //---
            builder.Entity<HER_CategoriaDocumento>()
                .HasOne(ug => ug.HER_Categoria)
                .WithMany(g => g.HER_Documentos)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_CategoriaDocumento>()
               .HasOne(ug => ug.HER_Documento)
               .WithMany(g => g.HER_Categorias)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_CategoriaDocumento>()
               .HasIndex(b => b.HER_CategoriaId)
               .IsUnique(false);

            builder.Entity<HER_CategoriaDocumento>()
                   .HasIndex(b => b.HER_DocumentoId)
                   .IsUnique(false);
            //--
            builder.Entity<HER_Documento>()
                .HasOne(p => p.HER_Tipo)
                .WithMany(b => b.HER_Documentos)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Documento>()
                .HasOne(p => p.HER_DocumentoTitular)
                .WithMany(b => b.HER_DocTitular)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Documento>()
                .HasOne(p => p.HER_DocumentoCreador)
                .WithMany(b => b.HER_DocCreador)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Documento>()
               .HasIndex(b => b.HER_TipoId)
               .IsUnique(false);

            builder.Entity<HER_Documento>()
               .HasIndex(b => b.HER_DocumentoTitularId)
               .IsUnique(false);

            builder.Entity<HER_Documento>()
               .HasIndex(b => b.HER_DocumentoCreadorId)
               .IsUnique(false);

            builder.Entity<HER_Destinatario>()
                .HasOne(ug => ug.HER_DocumentoBase)
                .WithMany(g => g.HER_Destinatarios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Destinatario>()
                .HasOne(ug => ug.HER_UDestinatario)
                .WithMany(g => g.HER_Destinatarios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Destinatario>()
                   .HasIndex(b => b.HER_UDestinatarioId)
                   .IsUnique(false);

            builder.Entity<HER_Destinatario>()
                   .HasIndex(b => b.HER_DocumentoBaseId)
                   .IsUnique(false);

            builder.Entity<HER_ConfiguracionUsuario>()
                .HasOne(ug => ug.HER_Color)
                .WithMany(g => g.HER_ConfiguracionUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_ConfiguracionUsuario>()
                .HasIndex(b => b.HER_UsuarioId)
                .IsUnique(false);

            builder.Entity<HER_ConfiguracionUsuario>()
                .HasIndex(b => b.HER_ColorId)
                .IsUnique(false);

            ////Color
            //builder.Entity<HER_Color>()
            //    .HasOne(p => p.HER_Coleccion)
            //    .WithMany(b => b.HER_Colores)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            ////Extension
            //builder.Entity<HER_Extension>()
            //    .HasOne(p => p.HER_Coleccion)
            //    .WithMany(b => b.HER_Extensiones)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Area>()
                .HasIndex(b => b.HER_LogoId)
                .IsUnique(false);

            builder.Entity<HER_InfoUsuario>()
                .HasOne(p => p.HER_Area)
                .WithMany(b => b.HER_Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Compromiso>()
                .HasOne(a => a.HER_Recepcion)
                .WithMany(g => g.HER_Compromiso)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Compromiso>()
                .HasIndex(b => b.HER_RecepcionId)
                .IsUnique(false);

            builder.Entity<HER_Recopilacion>()
                .HasOne(x => x.HER_Area)
                .WithOne(y => y.HER_Recopilacion)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<HER_Recopilacion>()
                .HasIndex(b => b.HER_AreaId)
                .IsUnique(false);

            //builder.Entity<HER_ConstanciaGeneral>();

            //builder.Entity<HER_ConstanciaSMDE>();

            //builder.Entity<HER_TipoConstancia>();

            //builder.Entity<HER_TipoPersonalConstancia>();

            //builder.Entity<HER_ConstanciaSeguimiento>();

            //builder.Entity<HER_TipoConstancia>();

            builder.Entity<HER_Bitacora>()
                .HasIndex(b => b.HER_InfoUsuarioId)
                .IsUnique(false);

            base.OnModelCreating(builder);
        }
    }
}
