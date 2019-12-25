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
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Annuncio> Annuncio { get; set; }
        public virtual DbSet<AnnunciPreferiti> AnnunciPreferiti { get; set; }
        public virtual DbSet<AnnuncioMessaggi> AnnuncioMessaggi { get; set; }
        public virtual DbSet<ClasseEnergetica> ClasseEnergetica { get; set; }
        public virtual DbSet<Comuni> Comuni { get; set; }
        public virtual DbSet<ImmagineAnnuncio> ImmagineAnnuncio { get; set; }
        public virtual DbSet<StatoProprieta> StatoProprieta { get; set; }
        public virtual DbSet<TipologiaAnnuncio> TipologiaAnnuncio { get; set; }
        public virtual DbSet<TipologiaProprieta> TipologiaProprieta { get; set; }
        public virtual DbSet<TipologiaRiscaldamento> TipologiaRiscaldamento { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<ImmaginePlanimetria> ImmaginePlanimetria { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<Appuntamento> Appuntamento { get; set; }
        public virtual DbSet<FasceOrarie> FasceOrarie { get; set; }
        public virtual DbSet<RicercheRecenti> RicercheRecenti { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<CodaNotifiche> CodaNotifiche { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Annuncio>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.AnnunciPreferiti)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.ImmagineAnnuncio)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.Video)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.ImmaginiPlanimetria)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.AnnuncioMessaggi)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClasseEnergetica>()
                .HasMany(e => e.Annuncio)
                .WithOptional(e => e.ClasseEnergetica)
                .HasForeignKey(e => e.IdClasseEnergetica);

            modelBuilder.Entity<Comuni>()
                .HasMany(e => e.Annuncio)
                .WithOptional(e => e.Comuni)
                .HasForeignKey(e => e.ComuneCodice);

            modelBuilder.Entity<Comuni>()
                .HasMany(e => e.RicercheRecenti)
                .WithRequired(e => e.Comune)
                .HasForeignKey(e => e.CodiceComune)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StatoProprieta>()
                .HasMany(e => e.Annuncio)
                .WithOptional(e => e.StatoProprieta)
                .HasForeignKey(e => e.IdStatoProprieta);

            modelBuilder.Entity<TipologiaAnnuncio>()
                .HasMany(e => e.Annuncio)
                .WithOptional(e => e.TipologiaAnnuncio)
                .HasForeignKey(e => e.IdTipologiaAnnuncio);

            modelBuilder.Entity<TipologiaProprieta>()
                .HasMany(e => e.Annuncio)
                .WithOptional(e => e.TipologiaProprieta)
                .HasForeignKey(e => e.IdTipologiaProprieta);

            modelBuilder.Entity<TipologiaRiscaldamento>()
                .HasMany(e => e.Annuncio)
                .WithOptional(e => e.TipologiaRiscaldamento)
                .HasForeignKey(e => e.IdTipologiaRiscaldamento);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.FasceOrarie)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);
            //.HasOptional(e => e.FasceOrarie)
            //.WithOptionalPrincipal(e => e.Annuncio);

            modelBuilder.Entity<Annuncio>()
                .HasMany(e => e.Appuntamenti)
                .WithRequired(e => e.Annuncio)
                .HasForeignKey(e => e.IdAnnuncio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.RicercheRecenti)
                .WithRequired(e => e.UserInfo)
                .HasForeignKey(e => e.IdAspNetUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.ChatDestinatario)
                .WithRequired(e => e.UserInfoDestinatario)
                .HasForeignKey(e => e.IdUserDestinatario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.ChatMittente)
                .WithRequired(e => e.UserInfoMittente)
                .HasForeignKey(e => e.IdUserMittente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chat>()
                .HasMany(e => e.AnnuncioMessaggi)
                .WithRequired(e => e.Chat)
                .HasForeignKey(e => e.IdChat)
                .WillCascadeOnDelete(false);


        }
    }
}
