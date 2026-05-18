using Microsoft.AspNetCore.Mvc; // Base pra criar API (Controller, rotas, etc)
using projetoIntegradorOlhuz.API.Services; // Importa o serviço da IA
using projetoIntegradorOlhuz.API.Models.DTO; // Importa os DTOs de leitura
using System;
using System.Threading.Tasks;

namespace projetoIntegradorOlhuz.API.Controllers
{
    // Define a rota base: api/IA
    [Route("api/[controller]")]
    [ApiController] // Diz pro ASP.NET que isso aqui é uma API REST
    public class IAController : ControllerBase
    {
        private readonly IConfiguration _configuration; // Acesso ao appsettings.json
        private readonly LeituraService _leituraService; // Serviço para salvar no banco

        // Construtor atualizado: recebe as configurações e o serviço de leitura via injeção de dependência
        public IAController(IConfiguration configuration, LeituraService leituraService)
        {
            _configuration = configuration;
            _leituraService = leituraService;
        }

        // Endpoint POST: api/IA/descrever
        // Recebe o arquivo da imagem e o ID do usuário que está fazendo a leitura
        [HttpPost("descrever")]
        public async Task<IActionResult> DescreverImagem([FromForm] IFormFile arquivo, [FromForm] int usuarioId)
        {
            // Validação básica: se não veio arquivo, já corta aqui
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Arquivo não enviado.");

            if (usuarioId <= 0)
                return BadRequest("Identificação do usuário inválida.");

            try
            {
                // Cria uma instância do serviço de IA (passa a config pra ele pegar a API Key)
                var visionService = new IAVisionServices(_configuration);

                // 1. Chama o método que manda a imagem pra IA
                var descricaoDescrita = await visionService.DescreverImagemAsync(arquivo);

                // 2. REGRA DE NEGÓCIO: Se a IA falhar ou o texto vier vazio, NÃO SALVA NO BANCO
                if (string.IsNullOrWhiteSpace(descricaoDescrita))
                {
                    return BadRequest(new { erro = "A leitura da imagem falhou ou nenhum dado foi detectado pela IA." });
                }

                // 3. Monta o DTO com o resultado bem-sucedido da IA
                var novoDto = new CriarLeituraDTO
                {
                    Resultado = descricaoDescrita,
                    UrlImagem = null // Pode ser preenchido futuramente caso salve o arquivo físico em disco/cloud
                };

                // 4. Salva a leitura de fato na tabela do Banco de Dados
                var leituraSalva = await _leituraService.SalvarLeituraAsync(novoDto, usuarioId);

                // Retorna sucesso (200 OK) com os dados salvos no banco e o resultado
                // Isso aqui é o que o front-end (JS) vai consumir para exibir na tela
                return Ok(leituraSalva);
            }
            catch (Exception ex)
            {
                // Loga o erro no console do servidor (Visual Studio)
                Console.WriteLine("ERRO CRÍTICO NA IA: " + ex.Message);

                // Qualquer erro no processo interrompe o fluxo e NADA é salvo no banco de dados
                return StatusCode(500, new { erro = ex.Message, detalhe = ex.InnerException?.Message });
            }
        }
    }
}