namespace PumpFit_Stock.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string Imagem { get; set; }

        public Produto()
        {
        }

        public Produto(string nome, int quantidade, string tamanho, string cor, string imagem)
        {
            Nome = nome;
            Quantidade = quantidade;
            Tamanho = tamanho;
            Cor = cor;
            Imagem = imagem;
        }
    }
}
