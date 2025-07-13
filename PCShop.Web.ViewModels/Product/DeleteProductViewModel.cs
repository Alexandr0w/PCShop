using System.ComponentModel.DataAnnotations;

namespace PCShop.Web.ViewModels.Product
{
    public class DeleteProductViewModel
    {
        [Required]
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string ImageUrl { get; set; }
    }
}
