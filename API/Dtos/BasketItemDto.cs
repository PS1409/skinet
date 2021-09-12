using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public string ProductName { get; set; }
         
        [Required]
        [Range(0.10, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1,10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }

        [Required]
        public string PictureUrl { get; set; } 

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Type { get; set; }
    }
}