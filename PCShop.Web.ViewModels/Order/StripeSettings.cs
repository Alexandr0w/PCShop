namespace PCShop.Web.ViewModels.Order
{
    public class StripeSettings
    {
        public string PublicKey { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;
    }
}
