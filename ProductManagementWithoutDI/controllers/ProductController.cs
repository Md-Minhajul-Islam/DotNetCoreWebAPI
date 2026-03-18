using Microsoft.AspNetCore.Mvc;
using ProductManagementWithoutDI.Models;
using ProductManagementWithoutDI.Services;

namespace ProductManagementWithoutDI.Controllers;

// Web API controller for managing products.
// This version demonstrates NO dependency injection:
// - The controller owns a ProductService instance.
// - In real-world apps, prefer DI for testability and flexibility.

[ApiController]
[Route("/product")]

public sealed class ProductController : ControllerBase
{
    // Option A (per-controller instance):
    // private readonly ProductService _service = new();
    
    // Option B (shared app-wide instance): use static (persists for process lifetime)
    // Using static here so the in-memory data doesn't reset every request.

    private static readonly ProductService _service = new();

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        // No DI: we call the service field directly
        var products =  _service.GetAll();
        return Ok(products);
    }

    //GET /api/products/{id} -> returns product by id.
    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _service.GetById(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product input)
    {
        try
        {
            var created = _service.Create(input);

            // Returns 201 with location header pointing to GET by id
            return CreatedAtAction(nameof(GetById), new
            {
                id = created.Id
            }, created);
        }catch(Exception ex)
        {
            return BadRequest(new {error = ex.Message});
        }
    }

    //PUT /api/products/{id} -> updates an existing product.
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Product input)
    {
        var ok = _service.Update(id, input);
        return ok ? NoContent() : NotFound();
    }

    // DELETE /api/products/{id} -> deletes an existing product.
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var ok = _service.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}