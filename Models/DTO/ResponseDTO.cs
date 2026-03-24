namespace projetoIntegradorOlhuz.API.Models.DTO
{
    public class ResponseDTO
    {
        public bool Erro { get; set; }
        public string Message { get; set; } = string.Empty;

        public Usuario? Usuario { get; set; }
    }
}
