﻿namespace PCShop.Web.ViewModels.Product
{
    public class DetailsProductViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public string? ImageUrl { get; set; }

        public required string Description { get; set; }

        public required string ProductType { get; set; }

        public decimal Price { get; set; }

        public required string CreatedOn { get; set; }
    }
}
