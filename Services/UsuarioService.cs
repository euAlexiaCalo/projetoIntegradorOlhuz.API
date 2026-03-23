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
    }
}
