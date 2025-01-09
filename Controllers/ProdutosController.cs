using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PumpFit_Stock.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PumpFit_Stock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly MappingProductService _mappingService;
        private readonly StockService _stockService;

        public ProdutosController(ProductService productService, MappingProductService mappingService, StockService stockService)
        {
            _productService = productService;
            _mappingService = mappingService;
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductAddDto>>> Get()
        {
            var produtos = await _productService.GetAllAsync();
            var produtosDto = produtos.Select(p => _mappingService.ToAddDto(p)).ToList();
            return Ok(produtosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAddDto>> GetById(int id)
        {
            var produto = await _productService.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }
            return Ok(_mappingService.ToAddDto(produto));
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<ProductAddDto>> Post([FromBody] ProductCreateDto produtoDto)
        {
            if (produtoDto == null)
            {
                return BadRequest("Produto inválido.");
            }

            var produto = _mappingService.FromCreateDto(produtoDto);
            await _productService.AddAsync(produto);

            var productAddDto = _mappingService.ToAddDto(produto);
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, productAddDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductCreateDto produtoDto)
        {
            var produtoExistente = await _productService.GetByIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound("Produto não encontrado.");
            }

            produtoExistente.Nome = produtoDto.Nome;
            produtoExistente.Quantidade = produtoDto.Quantidade;
            produtoExistente.Tamanho = produtoDto.Tamanho;
            produtoExistente.Cor = produtoDto.Cor;
            produtoExistente.Imagem = produtoDto.Imagem;

            await _productService.UpdateAsync(produtoExistente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _productService.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/estoque")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> AtualizarEstoque(int id, [FromBody] int quantidade)
        {
            var sucesso = await _stockService.AtualizarQuantidade(id, quantidade);
            if (!sucesso)
            {
                return NotFound("Produto não encontrado.");
            }

            return Ok($"Estoque atualizado com sucesso! Nova quantidade: {await _stockService.ObterQuantidadeDisponivel(id)}");
        }
    }
}
