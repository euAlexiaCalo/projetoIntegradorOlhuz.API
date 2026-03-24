using Microsoft.AspNetCore.Mvc;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Services;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriarContaController : ControllerBase
    {
        private readonly CriarContaService _criarContaService;

        public CriarContaController(CriarContaService criarContaService)
        {
            _criarContaService = criarContaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarConta([FromBody] CriarUsuarioDTO dadosUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDTO resultado = await _criarContaService.CriarConta(dadosUsuario);

            if (resultado.Erro)
            {
                // Retorna 400 Bad Request com a mensagem de erro específica
                return BadRequest(resultado);
            }

            // Se erro for falso retorna 200 OK
            return Ok(resultado);
        }
    }
}