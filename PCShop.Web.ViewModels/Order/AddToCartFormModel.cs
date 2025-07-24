using System.ComponentModel.DataAnnotations;
using static PCShop.Data.Common.ValidationMessageConstants.Order;

namespace PCShop.Web.ViewModels.Order
{
    public class AddToCartFormModel
    {
        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public string? ProductId { get; set; }

        public string? ComputerId { get; set; }

        [Range(MinQuantity, MaxQuantity, ErrorMessage = QuantityRange)]
        public int Quantity { get; set; } = 1;
    }
}
