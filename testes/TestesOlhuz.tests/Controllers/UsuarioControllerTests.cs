using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using projetoIntegradorOlhuz.API.Data;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Models.DTO;
using projetoIntegradorOlhuz.API.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestesOlhuz.tests.Controllers
{
    public class LoginServiceTests
    {
        // Helper para criar o banco em memória isolado
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        // Helper para criar um TokenService mockado
        private Mock<TokenService> GetTokenServiceMock()
        {
            var mockConfig = new Mock<IConfiguration>();
            // Configurações mínimas para o TokenService não quebrar
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("chave_mestra_super_secreta_123456789");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("teste");
            mockConfig.Setup(c => c["Jwt:Audience"]).Returns("teste");
            mockConfig.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");

            return new Mock<TokenService>(mockConfig.Object);
        }

        [Fact]
        public async Task Login_DeveRetornarSucesso_QuandoCredenciaisForemValidas()
        {
            // Arrange
            var db = GetDatabaseContext();
            var tokenServiceMock = GetTokenServiceMock();
            var loginService = new LoginService(db, tokenServiceMock.Object);

            // Criamos um usuário com senha criptografada (BCrypt)
            var senhaLimpa = "SenhaSegura123";
            var usuario = new Usuario
            {
                Nome = "Usuario Teste",
                Email = "login@teste.com",
                Senha = BCrypt.Net.BCrypt.HashPassword(senhaLimpa),
                CPF = "111.111.111-11"
            };
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();

            var loginDto = new LoginDTO { Email = "login@teste.com", Senha = senhaLimpa };

            // Act
            var resultado = await loginService.Login(loginDto);

            // Assert
            resultado.Erro.Should().BeFalse();
            resultado.Message.Should().Be("Login realizado com sucesso!");
            resultado.Token.Should().NotBeNullOrEmpty();
            resultado.Usuario.Email.Should().Be("login@teste.com");
        }

        [Fact]
        public async Task Login_DeveRetornarErro_QuandoSenhaForIncorreta()
        {
            // Arrange
            var db = GetDatabaseContext();
            var tokenServiceMock = GetTokenServiceMock();
            var loginService = new LoginService(db, tokenServiceMock.Object);

            var senhaReal = "SenhaCerta";
            db.Usuarios.Add(new Usuario
            {
                Email = "erro@teste.com",
                Senha = BCrypt.Net.BCrypt.HashPassword(senhaReal),
                Nome = "Teste Erro",
                CPF = "222.222.222-22"
            });
            await db.SaveChangesAsync();

            var loginDto = new LoginDTO { Email = "erro@teste.com", Senha = "SenhaErrada" };

            // Act
            var resultado = await loginService.Login(loginDto);

            // Assert
            resultado.Erro.Should().BeTrue();
            resultado.Message.Should().Be("E-mail ou senha incorretos.");
            resultado.Token.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Login_DeveRetornarErro_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var db = GetDatabaseContext();
            var tokenServiceMock = GetTokenServiceMock();
            var loginService = new LoginService(db, tokenServiceMock.Object);

            var loginDto = new LoginDTO { Email = "fantasma@teste.com", Senha = "123" };

            // Act
            var resultado = await loginService.Login(loginDto);

            // Assert
            resultado.Erro.Should().BeTrue();
            resultado.Message.Should().Be("E-mail ou senha incorretos.");
        }
    }
}