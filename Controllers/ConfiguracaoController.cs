using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.Models.DTO;
using projetoIntegradorOlhuz.API.Enum;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfiguracaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuracao>>> GetConfiguracoes()
        {
            return await _context.Configuracoes.ToListAsync();
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<Configuracao>> GetConfiguracaoPorUsuario(int usuarioId)
        {
            var config = await _context.Configuracoes
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (config == null) return NotFound(new { mensagem = "Configuração não encontrada para este usuário." });

            return config;
        }

        [HttpPost]
        public async Task<ActionResult<Configuracao>> PostConfiguracao(CriarConfiguracaoDTO dto)
        {
            var configuracao = new Configuracao
            {
                LeituraAtiva = dto.LeituraAtiva,
                VelocidadeLeitura = dto.VelocidadeLeitura,
                VozSintetica = dto.VozSintetica,
                Volume = dto.Volume,
                VibracaoAtiva = dto.VibracaoAtiva,
                ModoExibicao = dto.ModoExibicao,
                UsuarioId = dto.UsuarioId
            };

            _context.Configuracoes.Add(configuracao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConfiguracoes), new { id = configuracao.Id }, configuracao);
        }
    }
}