using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;


namespace projetoIntegradorOlhuz.API.Controllers
{
    //para habilitar a api
    [ApiController]

    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        [HttpPost("CriarUsuario")]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Usuario? usuarioEncontrado = await _usuarioDbContext,Usuario.FirsOrDeFaultAsync(usuario => Usuario.CPF == dadosUsuario.CPF);
          
            if (UsuarioEncontrado != null)
            {
                return BadRequest($"Já existe um usuario cadastrado com o CPF {dadosUsuario.CPF}");
            }

            Usuario usuario = new usuario
            {
                Nome = dadosUsuario.Nome,
                CPF = dadosUsuario.CPF,
                DataNascimento = dadosUsuario.DataNascimento,
                Telefone = dadosUsuario.Telefone,
                Email = dadosUsuario.Email,
                Senha = dadosUsuario.Senha
            };

            _usuarioDbContext.Usuario.Add(usuario);
            int resultadoGravacao = await _usuarioDbContext.SaveChangesAsync();

            if (resultadoGravacao > 0)
            {
                return Created();
            }

            return BadRequest("Erro ao criar usuario");
        }
    }
}
