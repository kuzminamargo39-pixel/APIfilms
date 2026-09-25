using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIfilms.Models.EntityFramework
{
    [Table("t_j_notation_not")]
    public class Notation
    {
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Column("flm_id")]
        public int FilmId { get; set; }

        [Required]
        [Column("not_note")]
        public int Note { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        [InverseProperty(nameof(Utilisateur.NotesUtilisateur))]
        public virtual Utilisateur UserNotator { get; set; } = null!;

        [ForeignKey(nameof(FilmId))]
        [InverseProperty(nameof(Film.NotesFilm))]
        public virtual Film MovieNote { get; set; } = null!;
    }
}