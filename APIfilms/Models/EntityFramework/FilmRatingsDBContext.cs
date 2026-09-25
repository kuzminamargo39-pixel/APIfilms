using Microsoft.EntityFrameworkCore;

namespace APIfilms.Models.EntityFramework
{
    public class FilmRatingsDBContext : DbContext
    {
        public FilmRatingsDBContext() { }

        public FilmRatingsDBContext(DbContextOptions<FilmRatingsDBContext> options)
            : base(options) { }

        public virtual DbSet<Film> Films { get; set; } = null!;
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
        public virtual DbSet<Notation> Notations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=FilmRatingsDB;Uid=postgres;Pwd=postgres;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notation>(entity =>
            {
                entity.HasKey(e => new { e.UtilisateurId, e.FilmId })
                      .HasName("pk_not");

                entity.ToTable(t => t.HasCheckConstraint("ck_not_note", "not_note between 0 and 5"));

                entity.HasOne(d => d.MovieNote)
                      .WithMany(p => p.NotesFilm)
                      .HasForeignKey(d => d.FilmId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_not_flm");

                entity.HasOne(d => d.UserNotator)
                      .WithMany(p => p.NotesUtilisateur)
                      .HasForeignKey(d => d.UtilisateurId)
                      .OnDelete(DeleteBehavior.Restrict) 
                      .HasConstraintName("fk_not_utl");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasIndex(e => e.Mail)
                      .IsUnique()
                      .HasDatabaseName("uq_utl_mail");

                entity.Property(e => e.Pays)
                      .HasDefaultValue("France");

                entity.Property(e => e.DateCreation)
                      .HasDefaultValueSql("now()");
            });
        }
    }
}