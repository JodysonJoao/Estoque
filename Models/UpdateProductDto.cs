using System.ComponentModel.DataAnnotations;

namespace PumpFit_Stock.Models
{
    public class ProductUpdateDto
    {
        [Required]
        public string Nome { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantidade { get; set; }

        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string Imagem { get; set; }
    }

}
