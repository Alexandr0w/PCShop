using System.ComponentModel.DataAnnotations;

namespace PCShop.Web.ViewModels.Order
{
    public class AddToCartFormModel
    {
        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public string? ProductId { get; set; }

        public string? ComputerId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
