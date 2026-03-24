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

            if (resultado.Erro)
                return Unauthorized(new { mensagem = resultado.Message });

            return Ok(new
            {
                usuario = resultado.Usuario,
                mensagem = resultado.Message
            });
        }
    }
}