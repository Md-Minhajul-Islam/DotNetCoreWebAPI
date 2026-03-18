namespace ProductManagementWithDI.Models
{
    // Represents a product item exposed by the API.
    public sealed class Product
    {
        public int Id { get; set; } // Unique Identifier
        public string Name { get; set; } = null!; // Product Name
        public decimal Price { get; set; } // Product Price
        public string? Description { get; set; } //Optional Product Description
        public string? Category { get; set; } //Optional Category Label (e.g., Electronics).
    }
}