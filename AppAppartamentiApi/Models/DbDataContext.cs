namespace AppAppartamentiApi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbDataContext : DbContext
    {
        public DbDataContext()
            : base("name=DbConnectionData")
        {
        }

        public virtual DbSet<Annuncio> Annuncio { get; set; }
        public virtual DbSet<AnnunciPreferiti> AnnunciPreferiti { get; set; }
        public virtual DbSet<ClasseEnergetica> ClasseEnergetica { get; set; }
        public virtual DbSet<ImmagineAnnuncio> ImmagineAnnuncio { get; set; }
        public virtual DbSet<StatoProprieta> StatoProprieta { get; set; }
        public virtual DbSet<TipologiaAnnuncio> TipologiaAnnuncio { get; set; }
        public virtual DbSet<TipologiaProprieta> TipologiaProprieta { get; set; }
        public virtual DbSet<TipologiaRiscaldamento> TipologiaRiscaldamentoe { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Annuncio>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.AnnunciPreferitis)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.ImmagineAnnuncios)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClasseEnergetica>()
                .HasMany(e => e.Annuncios)
                .WithOptional(e => e.ClasseEnergetica)
                .HasForeignKey(e => e.IdClasseEnergetica);

            modelBuilder.Entity<StatoProprieta>()
                .HasMany(e => e.Annuncios)
                .WithOptional(e => e.StatoProprieta)
                .HasForeignKey(e => e.IdStatoProprieta);

            modelBuilder.Entity<TipologiaAnnuncio>()
                .HasMany(e => e.Annuncios)
                .WithOptional(e => e.TipologiaAnnuncio)
                .HasForeignKey(e => e.IdTipologiaAnnuncio);

            modelBuilder.Entity<TipologiaProprieta>()
                .HasMany(e => e.Annuncios)
                .WithOptional(e => e.TipologiaProprieta)
                .HasForeignKey(e => e.IdTipologiaProprieta);

            modelBuilder.Entity<TipologiaRiscaldamento>()
                .HasMany(e => e.Annuncios)
                .WithOptional(e => e.TipologiaRiscaldamento)
                .HasForeignKey(e => e.IdTipologiaRiscaldamento);
        }
    }
}
