namespace projetoIntegradorOlhuz.API.Services
{

    public class ResultadoService<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T Dados { get; set; }


        public static ResultadoService<T> Ok(T dados, string mensagem = "")
        {
            return new ResultadoService<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }


        public static ResultadoService<T> Falha(string mensagem)
        {
            return new ResultadoService<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Dados = default
            };
        }
    }
}