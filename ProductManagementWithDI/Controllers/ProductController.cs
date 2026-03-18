using Microsoft.AspNetCore.Mvc;
using ProductManagementWithDI.Models;
using ProductManagementWithDI.Services;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("/product")]
    public sealed class ProductsController : ControllerBase
    {
        // Constructor Injection
        // This is the most common and recommended approach.
        // - The dependency is declared in the constructor.
        // - ASP.NET Core's DI container automatically supplies it when the controller is created.
        // - The injected instance (_service) is then available to all actions in this controller.
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET /api/products
        // Uses: Constructor-injected _service
        // Why: We need IProductService in multiple actions (CRUD),
        //      so constructor injection avoids duplication.
        // When to use: Any dependency required across multiple actions → default approach.
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _service.GetAll();
            return Ok(products);
        }

        // Action Method Injection
        // In this approach, a dependency is injected directly into a single action method
        // using the [FromServices] attribute.
        // - This keeps the constructor clean if the dependency is rarely used.
        // - The service exists only in this method's scope.
        // Real-world example: a reporting/export service needed only by one endpoint.

        // GET /api/products/{id}
        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(
            int id,
            [FromServices] IProductService productService // injected only for this action
        )
        {
            var product = productService.GetById(id);
            return product is null ? NotFound() : Ok(product);
        }

        // Manual Resolution (Service Locator)
        // This approach fetches a service manually from the built-in DI container
        // using HttpContext.RequestServices.
        // - This is discouraged in normal development (harder to test and maintain).
        // - It is shown here ONLY to contrast with the other two techniques.
        // - Sometimes useful in edge cases where constructor/action injection isn't possible
        //   (like dynamic resolution in middleware or factory patterns).

        // POST /api/products
        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product input)
        {
            // Manually resolve IProductService from the service provider
            var productService = HttpContext.RequestServices.GetRequiredService<IProductService>();

            try
            {
                var created = productService.Create(input);

                // Returns HTTP 201 (Created) with a Location header
                // pointing to the newly created resource (GetById).
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                // If service-level validation fails, return HTTP 400 (Bad Request).
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}