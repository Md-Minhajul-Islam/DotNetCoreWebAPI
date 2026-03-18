using Microsoft.AspNetCore.Razor.TagHelpers;
using ProductManagementWithoutDI.Models;

namespace ProductManagementWithoutDI.Services;

public sealed class ProductService
{
    // The in-memory "table" with hardcoded items.
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

    // Returns all products ordered by Id.   
    public IEnumerable<Product> GetAll()
    {
        return _store.OrderBy(p => p.Id).ToList();
    }

    // Return a single product by Id, or null if not found.
    public Product? GetById(int id)
    {
        return _store.FirstOrDefault(p => p.Id == id);
    }

    // Creates a new product with a generated Id.
    public Product Create(Product input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new ArgumentException("Product name is required.", nameof(input.Name));
        }
        if(input.Price < 0)
        {
            throw new ArgumentException("Price cannot be negative", nameof(input.Price));
        }
        // Generate newe Id (max existing Id+1)
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

    // Updates an existing product; returns false if the Id doesn't exist.
    // Only provided fields are applied; others are kept as-is
    public bool Update(int id, Product input)
    {
        var existing = _store.FirstOrDefault(p => p.Id == id);
        if(existing == null) return false;
        existing.Name = string.IsNullOrWhiteSpace(input.Name) ? existing.Name : input.Name.Trim();
        existing.Price = input.Price < 0 ? existing.Price : input.Price;
        existing.Category = string.IsNullOrWhiteSpace(input.Category) ? existing.Category : input.Category.Trim();
        existing.Description = string.IsNullOrWhiteSpace(input.Description) ? existing.Description : input.Description.Trim();

        return true;
    }

    // Deletes a prduct; return false if the Id doesn't exist.
    public bool Delete(int id)
    {
        var product = _store.FirstOrDefault(p => p.Id == id);
        if (product == null) return false;
        _store.Remove(product);
        return true;
    }
}