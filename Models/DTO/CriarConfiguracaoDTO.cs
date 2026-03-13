using projetoIntegradorOlhuz.API.Enum;

namespace projetoIntegradorOlhuz.Models.DTO
{
    public class CriarConfiguracaoDTO
    {
        public bool LeituraAtiva { get; set; }
        public double VelocidadeLeitura { get; set; }
        public VozSintetica VozSintetica { get; set; } 
        public int Volume { get; set; }
        public bool VibracaoAtiva { get; set; }
        public string ModoExibicao { get; set; } = "Escuro";
        public int UsuarioId { get; set; }
    }
}