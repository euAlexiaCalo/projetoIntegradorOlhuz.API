using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Services;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
<<<<<<< HEAD
       
        [HttpGet("Usuario")]
        [Authorize]
        public IActionResult mostrarDados()
=======
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpGet("Perfil")]
      
        public IActionResult ObterPerfil()
>>>>>>> main
        {
            
            var nomeUsuario = User.Identity?.Name;

            return Ok(new
            {
                mensagem = $"Bem-vindo(a), {nomeUsuario}"
            });
        }

        [HttpPost("criarUsuario")]
        public async Task<IActionResult> CriarUsarioAsync([FromBody] CriarUsuarioDTO dadosUsuario)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var resultado = await _usuarioService.criarUsuario(dadosUsuario);


            if (resultado.Erro)
                return BadRequest(resultado.Message);


            return Ok(new
            {
                mensagem = "usuário criado com sucesso"
            });
        }
    }
}