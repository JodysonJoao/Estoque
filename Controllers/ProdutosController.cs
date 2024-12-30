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
        public async Task<ActionResult<IEnumerable<ProductAddDTO>>> Get()
        {
            var produtos = await _context.Produtos
                .OrderBy(p => p.Id)
                .Select(p => new ProductAddDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Quantidade = p.Quantidade,
                    Tamanho = p.Tamanho,
                    Cor = p.Cor,
                    ImagemDTO = p.Imagem
                })
                .ToListAsync();

            return Ok(produtos);
        }




        [HttpPost]
        public async Task<ActionResult<ProductAddDTO>> Post([FromBody] ProductCreateDTO produtoDTO)
        {
            if (produtoDTO == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            var produto = new Produto
            {
                Nome = produtoDTO.Nome,
                Quantidade = produtoDTO.Quantidade,
                Tamanho = produtoDTO.Tamanho,
                Cor = produtoDTO.Cor,
                Imagem = produtoDTO.Imagem
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var productAddDTO = new ProductAddDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Quantidade = produto.Quantidade,
                Tamanho = produto.Tamanho,
                Cor = produto.Cor,
                ImagemDTO = produto.Imagem
            };

            return CreatedAtAction(nameof(Get), new { id = produto.Id }, productAddDTO);
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
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar produto: {ex.Message}");
                return StatusCode(500, "Erro ao deletar produto.");
            }

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
