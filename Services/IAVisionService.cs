using Newtonsoft.Json; // Biblioteca pra trabalhar com JSON (serializar e desserializar)
using System.Text;     // Usado aqui pra encoding UTF8

namespace projetoIntegradorOlhuz.API.Services
{
    // Serviço responsável por conversar com a IA do Google (Gemini)
    public class IAVisionServices
    {
        private readonly IConfiguration _configuration; // Acessa configs (tipo appsettings.json)
        private readonly HttpClient _httpClient;        // Cliente HTTP pra fazer requisições

        // Construtor: injeta configuração e cria o HttpClient
        public IAVisionServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        // Método principal: recebe uma imagem e devolve uma descrição em texto
        public async Task<string> DescreverImagemAsync(IFormFile arquivo)
        {
            // Pega a chave da API salva no appsettings.json
            var apiKey = _configuration["GeminiApiKey"];

            // URL da API do Gemini (modelo que aceita imagem)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            // Converte a imagem recebida em Base64 (porque a API não aceita arquivo direto)
            using var ms = new MemoryStream();
            await arquivo.CopyToAsync(ms); // copia o arquivo pra memória
            var base64Image = Convert.ToBase64String(ms.ToArray()); // transforma em texto Base64

            // Monta o "corpo" da requisição (payload) no formato que o Gemini espera
            var payload = new
            {
                contents = new[]
                {
                    new {
                        parts = new object[]
                        {
                            // Parte 1: instruções pra IA (o prompt)
                            new { text = @"Você é o assistente de voz da Olhus. Descreva esta imagem para uma pessoa cega seguindo REGRAS RÍGIDAS: 
                            1. NUNCA use negrito (**), itálico (*) ou listas com símbolos. Escreva apenas texto corrido com acentos e limpo.
                            2. Seja extremamente detalhado: identifique o que está na imagem. Se for um anime, diga o nome (ex: Naruto, One Piece). Se for uma série, identifique os atores ou a cena. Se for um carro, identifique a marca, modelo e ano se possível.
                            3. ADAPTAÇÃO DE VOZ: Como o texto será lido por um sintetizador, escreva palavras estrangeiras de forma fonética se necessário para que a pronúncia soe natural em português brasileiro.
                            4. Descreva cores, iluminação e a posição dos objetos (esquerda, direita, centro, fundo).
                            5. Se houver texto na imagem, transcreva-o integralmente dentro da narração." },

                            // Parte 2: a imagem em si (em Base64)
                            new { inline_data = new { mime_type = arquivo.ContentType, data = base64Image } }
                        }
                    }
                }
            };

            // Converte o objeto payload em JSON (string)
            var jsonPayload = JsonConvert.SerializeObject(payload);

            // Prepara o conteúdo da requisição HTTP (JSON + UTF8)
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Envia a requisição POST pro Google
            var response = await _httpClient.PostAsync(url, content);

            // Lê a resposta como texto
            var responseBody = await response.Content.ReadAsStringAsync();

            // Se deu tudo certo...
            if (response.IsSuccessStatusCode)
            {
                // Converte o JSON da resposta pra objeto dinâmico
                dynamic result = JsonConvert.DeserializeObject(responseBody);

                // Navega dentro do JSON gigante e pega só o texto da resposta da IA
                return result.candidates[0].content.parts[0].text;
            }

            // Se deu ruim, retorna o erro
            return "Erro ao falar com o Gemini: " + responseBody;
        }
    }
}