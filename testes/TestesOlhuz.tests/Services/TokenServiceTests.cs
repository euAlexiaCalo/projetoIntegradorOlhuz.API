using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using projetoIntegradorOlhuz.API.Models;
using projetoIntegradorOlhuz.API.Services;
using System;
using Xunit;

namespace TestesOlhuz.tests.Services
{
    public class TokenServiceTests
    {
        // Helper para simular o appsettings.json
        private IConfiguration GetMockConfiguration()
        {
            var mockConfig = new Mock<IConfiguration>();
            // Configura os valores que o seu TokenService vai pedir
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("ChaveSuperSecretaMuitoLongaParaTesteDeSeguranca123!");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("OlhuzIssuer");
            mockConfig.Setup(c => c["Jwt:Audience"]).Returns("OlhuzAudience");
            mockConfig.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");

            return mockConfig.Object;
        }

        [Fact]
        public void GenerateToken_QuandoUsuarioForValido_DeveRetornarJWTFormatadoCorretamente()
        {
            // Arrange -> Prepara configuração falsa e um usuário teste
            IConfiguration config = GetMockConfiguration();
            TokenService service = new TokenService(config);

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Silva",
                Email = "teste@olhuz.com"
            };

            // Act -> Gera o token
            string token = service.GenerateToken(usuario);

            // Assert -> Verifica os padrões do Token
            token.Should().NotBeNullOrWhiteSpace(); // O token não pode ser vazio

            // Um JWT válido sempre possui 3 partes separadas por um ponto (.)
            string[] partesDoToken = token.Split('.');
            partesDoToken.Length.Should().Be(3);
        }
    }
}