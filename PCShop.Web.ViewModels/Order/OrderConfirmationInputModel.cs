using System.ComponentModel.DataAnnotations;
using PCShop.Data.Models.Enum;
using static PCShop.Data.Common.EntityConstants.ApplicationUser;
using static PCShop.Data.Common.ValidationMessageConstants.Order;
using static PCShop.Data.Common.ValidationMessageConstants.ApplicationUser;

namespace PCShop.Web.ViewModels.Order
{
    public class OrderConfirmationInputModel
    {
        [Required(ErrorMessage = FullNameRequired)]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength, ErrorMessage = FullNameLength)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = AddressRequired)]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = AddressLength)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = CityRequired)]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = CityLength)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = PostalCodeRequired)]
        [StringLength(PostalCodeMaxLength, MinimumLength = PostalCodeMinLength, ErrorMessage = PostalCodeLength)]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = PhoneNumberRequired)]
        [Phone(ErrorMessage = PhoneNumberInvalid)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = EmailRequired)]
        [EmailAddress(ErrorMessage = EmailInvalid)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = DeliveryMethodRequired)]
        [EnumDataType(typeof(DeliveryMethod), ErrorMessage = DeliveryMethodInvalid)]
        public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.None;

        [Required(ErrorMessage = PaymentMethodRequired)]
        [EnumDataType(typeof(OrderPaymentMethod), ErrorMessage = PaymentMethodInvalid)]
        public OrderPaymentMethod PaymentMethod { get; set; } = OrderPaymentMethod.None;

        public string? Comment { get; set; }

        public decimal TotalProductsPrice { get; set; }
    }
}
