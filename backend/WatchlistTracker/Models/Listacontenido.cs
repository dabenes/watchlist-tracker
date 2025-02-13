using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchlistTracker.Models
{
    public class ListaContenido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ListaId { get; set; }

        [Required]
        public int PeliculaSerieId { get; set; }

        [ForeignKey("ListaId")]
        public Lista Lista { get; set; }

        [ForeignKey("PeliculaSerieId")]
        public PeliculaSerie PeliculaSerie { get; set; }
    }
}
