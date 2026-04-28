using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using projetoIntegradorOlhuz.API.Services;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace TestesOlhuz.tests.Services
{
    public class IAVisionServicesTests
    {
        [Fact]
        public async Task DescreverImagemAsync_QuandoAPIKeyForInvalida_DeveRetornarMensagemDeErro()
        {
            // Arrange -> Simula as configurações com uma chave API propositalmente falsa
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["GeminiApiKey"]).Returns("CHAVE_FALSA_PARA_NAO_GASTAR_CREDITOS");

            IAVisionServices service = new IAVisionServices(mockConfig.Object);

            // Arrange -> O truque de mestre: Simular um arquivo de imagem (IFormFile)
            var mockFile = new Mock<IFormFile>();
            var conteudoImagemMock = "dados_falsos_de_uma_imagem_jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(conteudoImagemMock));

            // Ensina o mock como se comportar quando o Service chamar "CopyToAsync"
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Callback<Stream, CancellationToken>((s, c) => stream.CopyTo(s))
                    .Returns(Task.CompletedTask);

            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

            // Act -> Dispara o método. Ele vai tentar bater no Google com a chave falsa e falhar
            string resultado = await service.DescreverImagemAsync(mockFile.Object);

            // Assert -> Verifica se o sistema segurou a falha com elegância, como você programou
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().Contain("Erro ao falar com o Gemini");
        }
    }
}