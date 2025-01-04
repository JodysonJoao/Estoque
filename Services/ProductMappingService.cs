using PumpFit_Stock.Models;

public class MappingProductService
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
            Imagem = produto.Imagem
        };
    }

    public Produto FromCreateDto(ProductCreateDto dto)
    {
        return new Produto
        {
            Nome = dto.Nome,
            Quantidade = dto.Quantidade,
            Tamanho = dto.Tamanho,
            Cor = dto.Cor,
            Imagem = dto.Imagem
        };
    }
}
