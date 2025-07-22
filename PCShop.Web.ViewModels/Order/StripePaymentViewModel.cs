using PCShop.Data.Models.Enum;

namespace PCShop.Web.ViewModels.Order
{
    public class StripePaymentViewModel
    {
        public decimal Amount { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StripePublicKey { get; set; } = string.Empty;

        public decimal TotalProductsPrice { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
