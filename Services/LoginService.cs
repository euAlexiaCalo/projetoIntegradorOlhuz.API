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


            bool isValidPassword = false;

            if (usuarioEncontrado != null)
            {
                // Só tenta verificar o Hash se encontrou o e-mail
                isValidPassword = BCrypt.Net.BCrypt.Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);
            }

            // Se o usuário não existir OU a senha for inválida
            if (usuarioEncontrado == null || !isValidPassword)
            {
                return new ResponseLoginDTO
                {
                    Erro = true,
                    Message = "E-mail ou senha incorretos."
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