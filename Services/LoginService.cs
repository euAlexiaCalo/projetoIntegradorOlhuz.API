using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Models.Response;

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

        public async Task<ResponseLoginDTO> Login(LoginDTO dadosUsuario)
        {

            // Busca o usuário pelo e-mail
            var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dadosUsuario.Email);


            if (usuarioEncontrado == null)
            {
                return new ResponseLoginDTO
                {
                    Erro = true,
                    Message = "Usuário não encontrado."
                };
            }

            // Válidar a senha
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

            if (!isValidPassword)
            {
                return new ResponseLoginDTO
                {
                    Erro = true,
                    Message = "Senha incorreta."
                };
            }

            var token = _tokenService.GenerateToken(usuarioEncontrado);

            return new ResponseLoginDTO
            {
                Erro = false,
                Message = "Login realizado com sucesso!",
                Token = token,
                Usuario = new Usuario
                {
                    Id = usuarioEncontrado.Id,
                    Nome = usuarioEncontrado.Nome,
                    Email = usuarioEncontrado.Email
                }
            };
        }
    }
}