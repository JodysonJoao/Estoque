namespace PumpFit_Stock.Models
{
    public class ProductAddDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string ImagemDTO { get; set; }

        public ProductAddDTO()
        {
        }

        public ProductAddDTO(int id, string nome, int quantidade, string tamanho, string cor, string imagemDTO)
        {
            Id = id;
            Nome = nome;
            Quantidade = quantidade;
            Tamanho = tamanho;
            Cor = cor;
            ImagemDTO = imagemDTO;
        }
    }
}
