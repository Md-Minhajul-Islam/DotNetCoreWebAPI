using ProductManagementWithDI.Services;

namespace ProductManagementWithDI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // DI mapping: interface -> concrete type
        // Singleton here so seeded data prsists across the app lifetime.
        builder.Services.AddSingleton<IProductService, ProductService>();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        

        app.Run();        
    }
}