using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;

namespace projetoIntegradorOlhuz.API.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> ObterUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return "Usuário não encontrado.";
            }
            return new
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                DataNascimento = usuario.DataNascimento,
                Telefone = usuario.Telefone,
                Email = usuario.Email
            };
        }

        public async Task<ResponseDTO> criarUsuario(CriarUsuarioDTO dadosUsuario) { 
            
            var usuarioExistente = await _context.Usuarios
                .AnyAsync(u => u.CPF == dadosUsuario.CPF || u.Email == dadosUsuario.Email);
            if (usuarioExistente)
            {
                return new ResponseDTO { 
                
                    Erro = true,
                    Message = "CPF ou Email já cadastrado.",

                };
            }
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dadosUsuario.Senha);
            var usuario = new Usuario
            {
                Nome = dadosUsuario.Nome,
                CPF = dadosUsuario.CPF,
                DataNascimento = dadosUsuario.DataNascimento,
                Telefone = dadosUsuario.Telefone,
                Email = dadosUsuario.Email,
                Senha = senhaHash
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return new ResponseDTO
            {
                Erro = false,
                Message = "Usuário cadastrado com sucesso!",

            };
        }
    }
}
