using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchlistTracker.Data;
using WatchlistTracker.Models;
using System.Security.Claims;

namespace WatchlistTracker.Controllers
{
    [Route("api/listas")]
    [ApiController]
    [Authorize] // Protege todas las rutas con JWT
    public class ListasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListasController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las listas del usuario autenticado
        [HttpGet]
        [Authorize]
        public IActionResult GetListas()
        {
            // Obtener el ID del usuario desde los claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Error al obtener el ID del usuario." });
            }

            var listas = _context.Listas.Where(l => l.UsuarioId == userId).ToList();
            return Ok(listas);
        }

        // Crear una nueva lista
        [HttpPost]
        public IActionResult CrearLista([FromBody] Lista lista)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            lista.UsuarioId = userId;
            _context.Listas.Add(lista);
            _context.SaveChanges();
            return Ok(new { message = "Lista creada correctamente", lista });
        }

        // Eliminar una lista
        [HttpDelete("{id}")]
        public IActionResult EliminarLista(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var lista = _context.Listas.FirstOrDefault(l => l.Id == id && l.UsuarioId == userId);

            if (lista == null)
                return NotFound(new { message = "Lista no encontrada" });

            _context.Listas.Remove(lista);
            _context.SaveChanges();
            return Ok(new { message = "Lista eliminada correctamente" });
        }
    }
}
