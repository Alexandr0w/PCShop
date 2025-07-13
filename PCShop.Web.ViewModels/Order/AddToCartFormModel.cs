using PCShop.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace PCShop.Web.ViewModels.Order
{
    public class AddToCartFormModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; } 

        public required string ImageUrl { get; set; } 

        public decimal Price { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
