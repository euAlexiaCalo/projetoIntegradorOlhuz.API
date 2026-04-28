using Microsoft.AspNetCore.Mvc; // Base pra criar API (Controller, rotas, etc)
using projetoIntegradorOlhuz.API.Services; // Importa o serviço da IA

namespace projetoIntegradorOlhuz.API.Controllers
{
    // Define a rota base: api/IA
    [Route("api/[controller]")]
    [ApiController] // Diz pro ASP.NET que isso aqui é uma API REST
    public class IAController : ControllerBase
    {
        private readonly IConfiguration _configuration; // Acesso ao appsettings.json

        // Construtor: recebe a configuração via injeção de dependência
        public IAController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Endpoint POST: api/IA/descrever
        [HttpPost("descrever")]
        public async Task<IActionResult> DescreverImagem(IFormFile arquivo)
        {
            // Validação básica: se não veio arquivo, já corta aqui
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Arquivo não enviado.");

            try
            {
                // Cria uma instância do serviço de IA
                // (passa a config pra ele pegar a API Key)
                var visionService = new IAVisionServices(_configuration);

                // Chama o método que manda a imagem pra IA
                var descricaoDescrita = await visionService.DescreverImagemAsync(arquivo);

                // Retorna sucesso (200 OK) com a descrição
                // Isso aqui é o que o front-end (JS) vai consumir
                return Ok(new { descricao = descricaoDescrita });
            }
            catch (System.Exception ex)
            {
                // Loga o erro no console do servidor (Visual Studio)
                Console.WriteLine("ERRO CRÍTICO NA IA: " + ex.Message);

                // Retorna erro 500 com detalhes
                // útil pra debug, perigoso em produção se não filtrar
                return StatusCode(500, new { erro = ex.Message, detalhe = ex.InnerException?.Message });
            }
        }
    }
}