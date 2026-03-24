namespace projetoIntegradorOlhuz.API.Models.DTO
{
    public class ResponseLoginDTO
    {
        public bool Erro { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
    }
}
