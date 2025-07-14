using System.ComponentModel.DataAnnotations;
using static PCShop.Data.Common.EntityConstants.Product;

namespace PCShop.Web.ViewModels.Product
{
    public class ProductFormInputModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.1, int.MaxValue)]
        public decimal Price { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public string ProductTypeId { get; set; } = null!;

        public IEnumerable<ProductTypeViewModel>? ProductTypes { get; set; }
    }
}
