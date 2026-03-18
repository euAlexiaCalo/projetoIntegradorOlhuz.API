using System.ComponentModel.DataAnnotations;

namespace projetoIntegradorOlhuz.API.Models
{
    public class Usuario
    {
        [Key] [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é um valor obrigatório")]
        // Define o limite máximo de caracteres para o banco e para a validação
        [StringLength(100, ErrorMessage = "O nome deve conter até 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é um valor obrigatório")]
        // Garante que o usuário digite exatamente 14 caracteres
        [StringLength(14, ErrorMessage = "CPF deve conter 14 caracteres no formato XXX.XXX.XXX-XX")]
        // Regex que valida a máscara: 3 dígitos, ponto, 3 dígitos, ponto, 3 dígitos, traço, 2 dígitos
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF deve seguir o formato 000.000.000-00")]
        public string CPF { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatorio")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [StringLength(250, ErrorMessage = "O e-mail deve ter no máximo 250 caracteres")]
        // Regex para validar se o e-mail possui '@', um domínio e um ponto
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O formato do e-mail é inválido (ex: usuario@dominio.com)")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é um valor obrigatorio")]
        public string Senha { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public Usuario()
        {
            DataCadastro = DateTime.UtcNow;
        }
    }
}
