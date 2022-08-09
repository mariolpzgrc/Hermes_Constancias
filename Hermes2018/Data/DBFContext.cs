using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hermes2018.ModelsDBF
{
    public partial class DBFContext : DbContext
    {
        public DBFContext()
        {
        }

        public DBFContext(DbContextOptions<DBFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CDEPEN> CDEPEN { get; set; }
        public virtual DbSet<CDEPEN_EQUIV> CDEPEN_EQUIV { get; set; }
        public virtual DbSet<HER_Constancias> HER_Constancias { get; set; }
        public virtual DbSet<HER_ConstanciaTipoPersonal> HER_ConstanciaTipoPersonal { get; set; }
        public virtual DbSet<HER_EstadoConstancias> HER_EstadoConstancias { get; set; }
        public virtual DbSet<HER_SolicitudConstancia> HER_SolicitudConstancia { get; set; }
        public virtual DbSet<HER_SolicitudConstanciaEstado> HER_SolicitudConstanciaEstado { get; set; }
        public virtual DbSet<HER_TipoPersonalConstancia> HER_TipoPersonalConstancia { get; set; }
        public virtual DbSet<PAREAS> PAREAS { get; set; }
        public virtual DbSet<PUSUARIOS> PUSUARIOS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(new Helpers.RestApiDSIA().GetConnection());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CDEPEN>(entity =>
            {
                entity.Property(e => e.NDEP).HasDefaultValueSql("('1')");

                entity.Property(e => e.NDEPA).HasDefaultValueSql("('1')");
            });

            modelBuilder.Entity<HER_Constancias>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<HER_TipoPersonalConstancia>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
