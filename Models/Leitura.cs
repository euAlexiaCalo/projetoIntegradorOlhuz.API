using System;
using System.ComponentModel.DataAnnotations;

namespace projetoIntegradorOlhuz.API.Models
{
    public class Leitura
    {
        [Key]
        public int Id { get; set; }

        // Inicializa como vazio para sumir o warning
        public string Resultado { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }

        // O "null!" avisa o compilador que o Entity Framework vai preencher essa propriedade depois
        public Usuario Usuario { get; set; } = null!;

        public string? UrlImagem { get; set; }
    }
}