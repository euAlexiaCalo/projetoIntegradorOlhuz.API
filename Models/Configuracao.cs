using System.ComponentModel.DataAnnotations;
using projetoIntegradorOlhuz.API.Enum;

namespace projetoIntegradorOlhuz.API.Models     
{
    public class Configuracao
    {
        [Key]
        public int Id { get; set; }
        public bool LeituraAtiva { get; set; }
        public double VelocidadeLeitura { get; set; }
        public VozSintetica VozSintetica { get; set; } // Masculina ou Feminina
        public int Volume { get; set; }
        public bool VibracaoAtiva { get; set; }
        public string ModoExibicao { get; set; } = "Escuro"; // Claro ou Escuro

        // Relacionamento: ID do usuário dono dessa configuração
        public int UsuarioId { get; set; }
    }
}

