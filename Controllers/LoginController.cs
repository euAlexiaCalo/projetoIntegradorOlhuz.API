using Microsoft.AspNetCore.Mvc;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Services;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _loginService.Login(login);

            if (!resultado.Sucesso)
                return Unauthorized(resultado.Mensagem);

            return Ok(new
            {
                dados = resultado.Dados,
                mensagem = resultado.Mensagem
            });
        }
    }
}