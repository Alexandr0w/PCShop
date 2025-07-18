using System.ComponentModel.DataAnnotations;

namespace PCShop.Web.ViewModels.Computer
{
    public class DeleteComputerViewModel
    {
        [Required]
        public required string Id { get; set; }

        public required string Name { get; set; }

        public string? ImageUrl { get; set; }
    }
}
