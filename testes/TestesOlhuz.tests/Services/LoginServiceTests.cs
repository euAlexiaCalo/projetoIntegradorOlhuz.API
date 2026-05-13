using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestesOlhuz.tests.Services
{
   
        // Ajustei o nome da classe para refletir o que está sendo testado
        public class UsuarioServiceTests
        {
            private AppDbContext GetDatabaseContext()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

                var databaseContext = new AppDbContext(options);
                databaseContext.Database.EnsureCreated();
                return databaseContext;
            }

            [Fact]
            public async Task ObterPerfil_DeveRetornarSucesso_QuandoUsuarioExiste()
            {
                // Arrange
                var db = GetDatabaseContext();
                var service = new UsuarioService(db);

                var usuario = new Usuario
                {
                    Id = 10,
                    Nome = "João Silva",
                    Email = "joao@email.com",
                    Senha = "hash_qualquer"
                };
                db.Usuarios.Add(usuario);
                await db.SaveChangesAsync();

                // Act
                var resultado = await service.ObterPerfil(10);

                // Assert
                resultado.Erro.Should().BeFalse();
                resultado.Message.Should().Be("Dados carregados com sucesso!");
                resultado.Usuario.Should().NotBeNull();
                resultado.Usuario!.Id.Should().Be(10);
                resultado.Usuario.Nome.Should().Be("João Silva");

                // Excelente! Isso garante que a senha não vaza para o front
                resultado.Usuario.Senha.Should().BeEmpty();
            }

            [Fact]
            public async Task ObterPerfil_DeveRetornarErro_QuandoUsuarioNaoExiste()
            {
                // Arrange
                var db = GetDatabaseContext();
                var service = new UsuarioService(db);

                // Act
                var resultado = await service.ObterPerfil(999);

                // Assert
                resultado.Erro.Should().BeTrue();
                resultado.Message.Should().Be("Usuário não encontrado.");
                resultado.Usuario.Should().BeNull();
            }
        }
    }