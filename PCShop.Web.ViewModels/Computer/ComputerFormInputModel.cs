using System.ComponentModel.DataAnnotations;
using static PCShop.Data.Common.EntityConstants.Computer;

namespace PCShop.Web.ViewModels.Computer
{
    public class ComputerFormInputModel
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

        public string? ImageUrl { get; set; }
    }
}
