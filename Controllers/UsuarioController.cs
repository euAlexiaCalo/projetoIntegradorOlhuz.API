using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;
using BCrypt.Net;
using projetoIntegradorOlhuz.API.Services;
using Microsoft.AspNetCore.Authorization; // Necessário para proteção de rotas

namespace projetoIntegradorOlhuz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _usuarioDbContext;
        private readonly TokenService _tokenService; // 1. Adicionado o serviço de token

        public UsuarioController(AppDbContext context, TokenService tokenService)
        {
            _usuarioDbContext = context;
            _tokenService = tokenService; // Inicializado via injeção de dependência
        }
        [Authorize]
        [HttpPost("CriarUsuario")]
        // Qualquer um pode criar conta
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDTO dadosUsuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioExistente = await _usuarioDbContext.Usuarios
                .AnyAsync(u => u.CPF == dadosUsuario.CPF || u.Email == dadosUsuario.Email);

            if (usuarioExistente)
            {
                return BadRequest("Usuário com este CPF ou E-mail já cadastrado.");
            }

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dadosUsuario.Senha);

            Usuario usuario = new Usuario
            {
                Nome = dadosUsuario.Nome,
                CPF = dadosUsuario.CPF,
                DataNascimento = dadosUsuario.DataNascimento,
                Telefone = dadosUsuario.Telefone,
                Email = dadosUsuario.Email,
                Senha = senhaHash,
            };

            _usuarioDbContext.Usuarios.Add(usuario);
            await _usuarioDbContext.SaveChangesAsync();

            return Created("", new { mensagem = "Usuário criado com sucesso!" });
        }

        [Authorize]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _usuarioDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha))
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

           
            var token = _tokenService.GenerateToken(usuario);

            return Ok(new
            {
                Token = token, 
                Usuario = new
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                },
                Mensagem = "Login realizado com sucesso!"
            });
        }


        [Authorize]
        [HttpGet("Perfil")]
        public IActionResult ObterPerfil()
        {
         
            var usuarioId = User.Identity?.Name;
            return Ok(new { mensagem = $"Bem-vindo, seu ID é {usuarioId}" });
        }
    }
}