using System;

namespace projetoIntegradorOlhuz.API.Models.DTO
{
    public class LeituraResponseDTO
    {
        public int Id { get; set; }
        public string Resultado { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public string? UrlImagem { get; set; }
    }
}