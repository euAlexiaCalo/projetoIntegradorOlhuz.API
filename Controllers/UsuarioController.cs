using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
       
        [HttpGet("Perfil")]
        [Authorize]
        public IActionResult ObterPerfil()
        {
            
            var usuarioId = User.Identity?.Name;

            return Ok(new
            {
                mensagem = $"Bem-vindo, seu ID é {usuarioId}"
            });
        }
    }
}