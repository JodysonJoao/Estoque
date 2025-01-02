namespace PumpFit_Stock.Models
{
    public class ProductAddDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string ImagemDto { get; set; }

        public ProductAddDto(Produto produto)
        {
            Id = produto.Id;
            Nome = produto.Nome;
            Quantidade = produto.Quantidade;
            Tamanho = produto.Tamanho;
            Cor = produto.Cor;
            ImagemDto = produto.Imagem;
        }

        public ProductAddDto()
        {
        }

        public ProductAddDto(int id, string nome, int quantidade, string tamanho, string cor, string imagemDto)
        {
            Id = id;
            Nome = nome;
            Quantidade = quantidade;
            Tamanho = tamanho;
            Cor = cor;
            ImagemDto = imagemDto;
        }
    }
}
