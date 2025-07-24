using System.ComponentModel.DataAnnotations;

namespace PCShop.Data.Models.Enum
{
    public enum DeliveryMethod
    {
        [Display(Name = "Select delivery method")]
        None = 0,
        Econt = 1,
        Speedy = 2,
        ToAddress = 3
    }
}
