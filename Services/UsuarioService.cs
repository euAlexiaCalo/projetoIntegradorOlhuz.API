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

        public async Task<ResponseDTO> ObterPerfil(int id)
        {
            var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return new ResponseDTO
                {
                    Erro = true,
                    Message = "Usuário não encontrado."
                };
            }

            usuario.Senha = string.Empty;

            return new ResponseDTO
            {
                Erro = false,
                Message = "Dados carregados com sucesso!",
                Usuario = usuario
            };
        }
    }
}
