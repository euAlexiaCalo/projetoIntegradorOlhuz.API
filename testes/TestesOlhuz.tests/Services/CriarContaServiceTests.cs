using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestesOlhuz.tests.Services
{
    public class CriarContaServiceTests
    {
        // Helper para criar o contexto do banco em memória isolado
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CriarConta_QuandoDadosForemValidos_DeveCadastrarComSucesso()
        {
            // Arrange
            var context = CreateContext();
            var service = new CriarContaService(context);

            // Criando o DTO com os novos campos (CPF e DataNascimento)
            var novoUsuarioDto = new CriarUsuarioDTO
            {
                Nome = "Ana Clara",
                CPF = "123.456.789-00",
                DataNascimento = new DateTime(2000, 1, 1),
                Email = "ana@olhuz.com",
                Senha = "SenhaForte123!"
            };

            // Act
            var resultado = await service.CriarConta(novoUsuarioDto);

            // Assert
            resultado.Erro.Should().BeFalse();
            resultado.Message.Should().Be("Usuário cadastrado com sucesso!");

            // Verifica se salvou no banco
            var usuarioNoBanco = context.Usuarios.FirstOrDefault(u => u.Email == "ana@olhuz.com");
            usuarioNoBanco.Should().NotBeNull();
            usuarioNoBanco!.Nome.Should().Be("Ana Clara");
            usuarioNoBanco.CPF.Should().Be("123.456.789-00");

            // Verifica se a senha foi criptografada (não pode ser igual à limpa)
            usuarioNoBanco.Senha.Should().NotBe("SenhaForte123!");
        }

        [Fact]
        public async Task CriarConta_QuandoEmailJaCadastrado_DeveRetornarErro()
        {
            // Arrange
            var context = CreateContext();

            // Adicionamos um usuário existente com o mesmo email
            context.Usuarios.Add(new Usuario
            {
                Nome = "Ja Existe",
                Email = "teste@email.com",
                Senha = "hash",
                CPF = "000.000.000-00"
            });
            await context.SaveChangesAsync();

            var service = new CriarContaService(context);
            var dtoDuplicado = new CriarUsuarioDTO
            {
                Nome = "Novo Usuario",
                CPF = "111.111.111-11",
                Email = "teste@email.com", // Mesmo email do anterior
                Senha = "123"
            };

            // Act
            var resultado = await service.CriarConta(dtoDuplicado);

            // Assert
            resultado.Erro.Should().BeTrue();
            resultado.Message.Should().Contain("já cadastrado");
            context.Usuarios.Count().Should().Be(1); // Não deve ter adicionado o segundo
        }
    }
}