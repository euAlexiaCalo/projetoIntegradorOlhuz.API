using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
       
        [HttpGet("Usuario")]
        [Authorize]
        public IActionResult mostrarDados()
        {
            
            var usuarioId = User.Identity?.Name;

            return Ok(new
            {
                mensagem = $"Bem-vindo, seu ID é {usuarioId}"
            });
        }
    }
}