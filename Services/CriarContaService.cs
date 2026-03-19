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

        public async Task<ResultadoService<string>> CriarUsuario(CriarUsuarioDTO dadosUsuario)
        {
            
            var usuarioExistente = await _context.Usuarios
                .AnyAsync(u => u.CPF == dadosUsuario.CPF || u.Email == dadosUsuario.Email);

            if (usuarioExistente)
            {
                return ResultadoService<string>.Falha("Usuário com este CPF ou E-mail já cadastrado.");
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


            return ResultadoService<string>.Ok("", "Usuário criado com sucesso!");
        }
    }
}