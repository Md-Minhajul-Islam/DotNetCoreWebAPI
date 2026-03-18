using ProductManagementWithDI.Models;

namespace ProductManagementWithDI.Services;

// In-memory implementation of IProductService.
// Uses a List<Product> with inline seed data.
// Thread-safety note: we guard all access with a simple lock (_lock)
// because we register this as a Singleton for demo persistence.

public sealed class ProductService : IProductService
{
    private readonly List<Product> _store =
    [
        new Product
            {
                Id = 1,
                Name = "Notebook",
                Price = 199.00m,
                Description = "A ruled paper notebook (100 pages).",
                Category = "Stationery"
            },
            new Product
            {
                Id = 2,
                Name = "Wireless Mouse",
                Price = 499.00m,
                Description = "2.4 GHz wireless mouse with ergonomic design.",
                Category = "Electronics"
            }
    ];

    // simple lock to protect _store when running as a Singleton
    private readonly object _lock = new();
    public IEnumerable<Product> GetAll()
    {
        lock (_lock)
        {
            return _store.OrderBy(p => p.Id).ToList();
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            return _store.FirstOrDefault(p => p.Id == id);
        }
    }

    public Product Create(Product input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new ArgumentException("Product name is required.", nameof(input.Name));
            if (input.Price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(input.Price));
            lock (_lock)
            {
                // Generate new Id (max existing Id + 1)
                var newId = _store.Any() ? _store.Max(p => p.Id) + 1 : 1;
                var product = new Product
                {
                    Id = newId,
                    Name = input.Name.Trim(),
                    Price = input.Price,
                    Category = string.IsNullOrWhiteSpace(input.Category) ? null : input.Category.Trim(),
                    Description = string.IsNullOrWhiteSpace(input.Description) ? null : input.Description.Trim()
                };
                _store.Add(product);
                return product;
            }
        }
        public bool Update(int id, Product input)
        {
            lock (_lock)
            {
                var existing = _store.FirstOrDefault(p => p.Id == id);
                if (existing == null) return false;
                // Only update provided fields
                existing.Name = string.IsNullOrWhiteSpace(input.Name) ? existing.Name : input.Name.Trim();
                existing.Price = input.Price < 0 ? existing.Price : input.Price;
                existing.Category = string.IsNullOrWhiteSpace(input.Category) ? existing.Category : input.Category.Trim();
                existing.Description = string.IsNullOrWhiteSpace(input.Description) ? existing.Description : input.Description.Trim();
                return true;
            }
        }
        public bool Delete(int id)
        {
            lock (_lock)
            {
                var product = _store.FirstOrDefault(p => p.Id == id);
                if (product == null) return false;
                _store.Remove(product);
                return true;
            }
        }
}