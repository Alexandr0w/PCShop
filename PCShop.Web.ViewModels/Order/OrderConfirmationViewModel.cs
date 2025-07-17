using System.ComponentModel.DataAnnotations;
using PCShop.Data.Models.Enum;
using static PCShop.Data.Common.EntityConstants.ApplicationUser;

namespace PCShop.Web.ViewModels.Order
{
    public class OrderConfirmationViewModel
    {
        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(PostalCodeMaxLength, MinimumLength = PostalCodeMinLength)]
        public string PostalCode { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.None;

        public string? Comment { get; set; }

        public decimal TotalProductsPrice { get; set; }
    }
}