using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlhuzApiWeb.Data;
using OlhuzApiWeb.Model;
using OlhuzApiWeb.Model.DTO;

namespace OlhuzApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfiguracoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuracao>>> GetConfiguracoes()
        {
            return await _context.Configuracao.ToListAsync();
        }

  
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<Configuracao>> GetConfiguracaoPorUsuario(int usuarioId)
        {
            var config = await _context.Configuracao
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

            _context.Configuracao.Add(configuracao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConfiguracoes), new { id = configuracao.Id }, configuracao);
        }
    }
}