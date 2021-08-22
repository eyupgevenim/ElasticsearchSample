using System.Collections.Generic;

namespace Search.API.Models
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> Tags { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
    }

    public class ProductProperty
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
