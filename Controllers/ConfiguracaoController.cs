using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Enum;

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private Configuracao configuracoes;

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
            // 1. Primeiro, verificamos se esse usuário já tem alguma configuração salva
            var configExistente = await _context.Configuracoes
                .FirstOrDefaultAsync(c => c.UsuarioId == dto.UsuarioId);

            if (configExistente != null)
            {
                // 2. Se JÁ EXISTE, nós apenas atualizamos os valores daquela linha
                configExistente.LeituraAtiva = dto.LeituraAtiva;
                configExistente.VelocidadeLeitura = dto.VelocidadeLeitura;
                configExistente.VozSintetica = dto.VozSintetica;
                configExistente.Volume = dto.Volume;
                configExistente.VibracaoAtiva = dto.VibracaoAtiva;
                configExistente.ModoExibicao = dto.ModoExibicao;

                _context.Configuracoes.Update(configExistente);
                await _context.SaveChangesAsync();

                return Ok(configExistente);
            }
            else
            {
                // 3. Se NÃO EXISTE (primeira vez do usuário), criamos uma nova
                var novaConfig = new Configuracao
                {
                    LeituraAtiva = dto.LeituraAtiva,
                    VelocidadeLeitura = dto.VelocidadeLeitura,
                    VozSintetica = dto.VozSintetica,
                    Volume = dto.Volume,
                    VibracaoAtiva = dto.VibracaoAtiva,
                    ModoExibicao = dto.ModoExibicao,
                    UsuarioId = dto.UsuarioId
                };

                _context.Configuracoes.Add(novaConfig); // Agora usando a variável certa!
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetConfiguracoes), new { id = novaConfig.Id }, novaConfig);
            }
        }
    }
}
