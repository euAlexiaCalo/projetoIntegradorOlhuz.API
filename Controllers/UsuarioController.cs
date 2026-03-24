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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerfil(int id)
        {
            var resultado = await _usuarioService.ObterPerfil(id);

            if (resultado.Erro)
            {
                return NotFound(resultado); // Retorna 404 se não achar
            }

            return Ok(resultado); // Retorna 200 com os dados
        }
    }
}