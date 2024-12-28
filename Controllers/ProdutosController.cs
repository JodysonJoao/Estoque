using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PumpFit_Stock.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumpFit_Stock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return await _context.Produtos.OrderBy(p => p.Id).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post([FromBody] Produto produto)
        {
            if (produto == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            var produtoExistente = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Nome == produto.Nome && p.Tamanho == produto.Tamanho && p.Cor == produto.Cor);

            if (produtoExistente != null)
            {
                return Conflict("Produto já existe.");
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest("ID do produto não corresponde ao ID da URL.");
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound("Produto não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            await ReordenarIds();
            return NoContent();
        }

        private async Task ReordenarIds()
        {
            var produtos = await _context.Produtos.OrderBy(p => p.Id).ToListAsync();

            for (int i = 0; i < produtos.Count; i++)
            {
                produtos[i].Id = i + 1;
                _context.Entry(produtos[i]).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
