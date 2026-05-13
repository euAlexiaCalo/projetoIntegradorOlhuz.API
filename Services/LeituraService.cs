using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO; // Importa os DTOs que criamos
using projetoIntegradorOlhuz.API.Data;       // Importa o AppDbContext

namespace projetoIntegradorOlhuz.API.Services
{
    public class LeituraService
    {
        private readonly AppDbContext _context;

        // O Construtor faz o Visual Studio entender o que é o "_context"
        public LeituraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LeituraResponseDTO> SalvarLeituraAsync(CriarLeituraDTO dto, int usuarioId)
        {
            var novaLeitura = new Leitura
            {
                Resultado = dto.Resultado,
                UsuarioId = usuarioId,
                DataCriacao = DateTime.UtcNow,
                UrlImagem = dto.UrlImagem
            };

            _context.Leituras.Add(novaLeitura);
            await _context.SaveChangesAsync();

            // Mapeia os dados salvos de volta para o formato de resposta
            return new LeituraResponseDTO
            {
                Id = novaLeitura.Id,
                Resultado = novaLeitura.Resultado,
                DataCriacao = novaLeitura.DataCriacao,
                UsuarioId = novaLeitura.UsuarioId,
                UrlImagem = novaLeitura.UrlImagem
            };
        }
    }
}