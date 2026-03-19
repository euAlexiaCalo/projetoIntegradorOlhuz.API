using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;

namespace projetoIntegradorOlhuz.API.Services
{
    public class LoginService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public LoginService(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ResultadoService<object>> Login(LoginDTO login)
        {
           
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email);

          
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha))
            {
                return ResultadoService<object>.Falha("E-mail ou senha inválidos.");
            }

        
            var token = _tokenService.GenerateToken(usuario);

          
            var dados = new
            {
                Token = token,
                Usuario = new
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            };

           
            return ResultadoService<object>.Ok(dados, "Login realizado com sucesso!");
        }
    }
}