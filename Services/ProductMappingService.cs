using PumpFit_Stock.Models;

public class ProductMappingService
{
    public ProductAddDto ToAddDto(Produto produto)
    {
        return new ProductAddDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Quantidade = produto.Quantidade,
            Tamanho = produto.Tamanho,
            Cor = produto.Cor,
            ImagemDto = produto.Imagem
        };
    }
}
