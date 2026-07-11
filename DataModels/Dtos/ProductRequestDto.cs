using System.ComponentModel.DataAnnotations;

namespace DataModels.Dtos
{
    // Gilt für Create UND Update - enthält bewusst kein Id-Feld,
    // die Id wird nie vom Client vorgegeben.
    public class ProductRequestDto
    {
        [Required(ErrorMessage = "Name ist erforderlich")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name muss zwischen 1 und 200 Zeichen lang sein")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price muss größer als 0 sein")]
        public decimal Price { get; set; }
    }
}
