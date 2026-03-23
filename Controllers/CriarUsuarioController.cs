using Microsoft.AspNetCore.Mvc;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Services;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriarContaController : Controller
    {
        private readonly CriarContaService _criarContaService;

        public CriarContaController(CriarContaService criarContaService)
        {
            _criarContaService = criarContaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDTO dadosUsuario)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var resultado = await _criarContaService.CriarUsuario(dadosUsuario);

            
            if (!resultado.Sucesso)
                return BadRequest(resultado.Mensagem);

            
            return Created("", new
            {
                mensagem = resultado.Mensagem
            });
        }
    }
}