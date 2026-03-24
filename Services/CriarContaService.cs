using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;

namespace projetoIntegradorOlhuz.API.Services
{
    public class CriarContaService
    {
        private readonly AppDbContext _context;

        public CriarContaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO> CriarConta(CriarUsuarioDTO dadosUsuario)
        {
            try
            {
                // Verifica se já existe um usuário com o mesmo CPF ou Email
                var usuarioExistente = await _context.Usuarios
                    .AnyAsync(u => u.CPF == dadosUsuario.CPF || u.Email == dadosUsuario.Email);

                if (usuarioExistente)
                {
                    return new ResponseDTO
                    {
                        Erro = true,
                        Message = "CPF ou Email já cadastrado."
                    };
                }

                // Criptografa a senha
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(dadosUsuario.Senha);

                // Mapeia o DTO para a Entidade Usuario
                var usuario = new Usuario
                {
                    Nome = dadosUsuario.Nome,
                    CPF = dadosUsuario.CPF,
                    DataNascimento = dadosUsuario.DataNascimento,
                    Telefone = dadosUsuario.Telefone,
                    Email = dadosUsuario.Email,
                    Senha = senhaHash
                };

                // Salva no Banco de Dados
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Limpa a senha para segurança antes de retornar o objeto
                usuario.Senha = string.Empty;

                return new ResponseDTO
                {
                    Erro = false,
                    Message = "Usuário cadastrado com sucesso!",
                    Usuario = usuario
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Erro = true,
                    Message = "Erro interno ao tentar cadastrar o usuário: " + ex.Message
                };
            }
        }
    }
}