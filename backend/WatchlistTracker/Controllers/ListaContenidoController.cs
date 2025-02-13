using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchlistTracker.Data;
using WatchlistTracker.Models;
using System.Security.Claims;

namespace WatchlistTracker.Controllers
{
    [Route("api/lista-contenido")]
    [ApiController]
    [Authorize] // Protege todas las rutas con JWT
    public class ListaContenidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListaContenidoController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las películas y series de una lista
        [HttpGet("{listaId}")]
        public IActionResult GetListaContenido(int listaId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var lista = _context.Listas.FirstOrDefault(l => l.Id == listaId && l.UsuarioId == userId);
            if (lista == null)
                return NotFound(new { message = "Lista no encontrada o no pertenece al usuario" });

            var contenido = _context.ListasContenidos
                .Where(lc => lc.ListaId == listaId)
                .Select(lc => lc.PeliculaSerie)
                .ToList();

            return Ok(contenido);
        }

        // Agregar una película o serie a una lista
        [HttpPost]
        public IActionResult AgregarALista([FromBody] ListaContenido listaContenido)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var lista = _context.Listas.FirstOrDefault(l => l.Id == listaContenido.ListaId && l.UsuarioId == userId);
            if (lista == null)
                return NotFound(new { message = "Lista no encontrada o no pertenece al usuario" });

            var peliculaSerie = _context.PeliculasSeries.FirstOrDefault(p => p.Id == listaContenido.PeliculaSerieId);
            if (peliculaSerie == null)
                return NotFound(new { message = "Película/Serie no encontrada" });

            _context.ListasContenidos.Add(listaContenido);
            _context.SaveChanges();
            return Ok(new { message = "Película/Serie añadida a la lista correctamente" });
        }

        // Eliminar una película o serie de una lista
        [HttpDelete("{listaId}/{peliculaSerieId}")]
        public IActionResult EliminarDeLista(int listaId, int peliculaSerieId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var lista = _context.Listas.FirstOrDefault(l => l.Id == listaId && l.UsuarioId == userId);
            if (lista == null)
                return NotFound(new { message = "Lista no encontrada o no pertenece al usuario" });

            var listaContenido = _context.ListasContenidos.FirstOrDefault(lc => lc.ListaId == listaId && lc.PeliculaSerieId == peliculaSerieId);
            if (listaContenido == null)
                return NotFound(new { message = "Película/Serie no encontrada en la lista" });

            _context.ListasContenidos.Remove(listaContenido);
            _context.SaveChanges();
            return Ok(new { message = "Película/Serie eliminada de la lista correctamente" });
        }
    }
}
