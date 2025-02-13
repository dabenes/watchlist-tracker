using System.ComponentModel.DataAnnotations;

namespace WatchlistTracker.Models
{
    public class PeliculaSerie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Categoria { get; set; } = string.Empty; // Acción, Comedia, etc.

        [Required]
        public string Tipo { get; set; } = "pelicula"; // "pelicula" o "serie"

        public int? Temporadas { get; set; } // Solo si es una serie

        public int? CapitulosPorTemporada { get; set; } // Solo si es una serie
    }
}
