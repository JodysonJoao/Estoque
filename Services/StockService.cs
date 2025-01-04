using PumpFit_Stock.Models;

public class StockService
{
    private readonly ApplicationDbContext _context;

    public StockService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AtualizarQuantidade(int produtoId, int quantidade)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return false;

        produto.Quantidade += quantidade;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> ObterQuantidadeDisponivel(int produtoId)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        return produto?.Quantidade ?? 0;
    }
}
