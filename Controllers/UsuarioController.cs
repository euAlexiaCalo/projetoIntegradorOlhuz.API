using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projetoIntegradorOlhuz.API.Services;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpGet("Usuario")]
        public IActionResult mostrarDados()
        {
            // Puxa o nome do usuário autenticado pelo Token JWT
            var nomeUsuario = User.Identity?.Name;

            if (string.IsNullOrEmpty(nomeUsuario))
            {
                return Unauthorized("Usuário não identificado.");
            }

            return Ok(new
            {
                mensagem = $"Bem-vindo(a), {nomeUsuario}",
                dataConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            });
        }
    }
}