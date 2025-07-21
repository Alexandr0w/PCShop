using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static PCShop.Data.Common.EntityConstants.Product;
using static PCShop.Data.Common.ValidationMessageConstants.Common;

namespace PCShop.Web.ViewModels.Product
{
    public class ProductFormInputModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = NameRequired)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequired)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = PriceRequired)]
        [Range(PriceMinValue, PriceMaxValue, ErrorMessage = PriceRange)]
        public decimal Price { get; set; }


        [Required(ErrorMessage = CreatedOnRequired)]
        [StringLength(CreatedOnLength, MinimumLength = CreatedOnLength, ErrorMessage = CreatedOnNeededLength)]
        public string CreatedOn { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlLength)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = ProductTypeRequired)]
        public string ProductTypeId { get; set; } = null!;

        public IEnumerable<ProductTypeViewModel>? ProductTypes { get; set; }
    }
}
