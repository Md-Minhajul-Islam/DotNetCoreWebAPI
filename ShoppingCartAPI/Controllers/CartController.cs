
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Services;

namespace ShoppingCartAPI.Controllers;

// Marks this as a Web API controller
// [ApiController] enables automatic model validation and better request handling
[ApiController]
// Base route -> all endpoints will be under "api/cart"
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    // Injected dependency for cart operations (Scoped service)
    private readonly ICartService _cartService;

    // Constructor Injection
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public IActionResult AddItem(CartItem item)
    {
        _cartService.AddItem(item);
        return Ok(new {Message = $"{item.ProductName} add to cart."});
    }

    [HttpGet("items")]
    public IActionResult GetItems()
    {
        return Ok(_cartService.GetItems());
    }

    [HttpGet("Summary")]
    public IActionResult GetSummary([FromServices] ICartSummaryService summaryService)
    {
        // Here, CartSummaryService is injected per-request (Scoped)
        // It internally uses:
        //   - ICartService (Scoped, manages cart data)
        //   - IDiscountService (Transient, two different instances for discount calculations)
        //   - IAppConfigService (Singleton, global tax & delivery fee rules)
        var summary = summaryService.GenerateSummary();
        return Ok(summary);
    }

    [HttpDelete("clear")]
    public IActionResult ClearCart()
    {
        _cartService.ClearCart();
        return Ok(new {Message = "Cart cleard"});
    }
}
