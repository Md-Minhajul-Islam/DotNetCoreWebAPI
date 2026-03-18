using ShoppingCartAPI.Services;

namespace ShoppingCartAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();        

        // Service Register lifetimes
        // Register AppConfigService as Singleton
        // One instanc is created ans shared across the entire application lifetime
        // - Good for global config like tax rate, delivery fee
        // - Created once, reused everywhere
        builder.Services.AddSingleton<IAppConfigService, AppConfigService>();

        // Register CartService as Scoped
        // One instance per HTTP request
        //    - Each request gets its own CartService instance
        //    - Items stored in CartService are isolated to that request
        //    - Demonstrates per-request lifetime
        builder.Services.AddScoped<ICartService, CartService>(); 

        // Register DiscountService as Transient
        // A new instance is created every time it is requested
        //    - Even within the same request, multiple injections create different objects
        //    - Great for lightweight, stateless operations like discount calculations
        builder.Services.AddTransient<IDiscountService, DiscountService>();
        
        // Register CartSummaryService as Scoped
        // New instance per HTTP request
        //    - Depends on CartService, DiscountService, AppConfigService
        //    - Calculates cart totals (subtotal, discount, tax, delivery, final total)
        //    - Scoped makes sense because summary is tied to the current cart/request
        builder.Services.AddScoped<ICartSummaryService, CartSummaryService>();

        // Register HttpContextAccessor
        // Provides access to HttpContext (e.g., reading headers like "UserId")
        //    - Needed for CartService to know which user's cart to manage
        builder.Services.AddHttpContextAccessor();

        // Register In-Memory Cache
        // Provides IMemoryCache (Singleton under the hood)
        //    - Used by CartService to persist cart data across requests for a given user
        //    - Supports expiration policies (e.g., cart expires after 30 minutes of inactivity)
        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}