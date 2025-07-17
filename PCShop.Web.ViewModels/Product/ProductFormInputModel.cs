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
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }


        [Required]
        [StringLength(CreatedOnLength, MinimumLength = CreatedOnLength)]
        public string CreatedOn { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        public string ProductTypeId { get; set; } = null!;

        public IEnumerable<ProductTypeViewModel>? ProductTypes { get; set; }
    }
}
