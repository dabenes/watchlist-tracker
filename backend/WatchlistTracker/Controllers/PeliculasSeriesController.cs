using Microsoft.AspNetCore.Mvc;
using WatchlistTracker.Data;
using WatchlistTracker.Models;

namespace WatchlistTracker.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasSeriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeliculasSeriesController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las películas y series
        [HttpGet]
        public IActionResult GetPeliculasSeries()
        {
            var peliculasSeries = _context.PeliculasSeries.ToList();
            return Ok(peliculasSeries);
        }

        // Obtener detalles de una película o serie por ID
        [HttpGet("{id}")]
        public IActionResult GetPeliculaSerie(int id)
        {
            var peliculaSerie = _context.PeliculasSeries.FirstOrDefault(p => p.Id == id);
            if (peliculaSerie == null)
                return NotFound(new { message = "Película/Serie no encontrada" });

            return Ok(peliculaSerie);
        }

        // Agregar una nueva película o serie
        [HttpPost]
        public IActionResult AgregarPeliculaSerie([FromBody] PeliculaSerie peliculaSerie)
        {
            _context.PeliculasSeries.Add(peliculaSerie);
            _context.SaveChanges();
            return Ok(new { message = "Película/Serie agregada correctamente", peliculaSerie });
        }

        // Eliminar una película o serie
        [HttpDelete("{id}")]
        public IActionResult EliminarPeliculaSerie(int id)
        {
            var peliculaSerie = _context.PeliculasSeries.FirstOrDefault(p => p.Id == id);
            if (peliculaSerie == null)
                return NotFound(new { message = "Película/Serie no encontrada" });

            _context.PeliculasSeries.Remove(peliculaSerie);
            _context.SaveChanges();
            return Ok(new { message = "Película/Serie eliminada correctamente" });
        }
    }
}
