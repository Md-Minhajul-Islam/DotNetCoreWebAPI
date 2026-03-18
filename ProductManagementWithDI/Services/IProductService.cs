using ProductManagementWithDI.Models;

namespace ProductManagementWithDI.Services;


// Defines the contract for product operations
public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Create(Product input);
    bool Update(int id, Product input);
    bool Delete(int id);
}