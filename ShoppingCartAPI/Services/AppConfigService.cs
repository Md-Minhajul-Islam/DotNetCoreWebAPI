namespace ShoppingCartAPI.Services;

public class AppConfigService : IAppConfigService
{
    private readonly decimal _taxRate = 0.18m; // GST rate = 18%
    
    public AppConfigService(ILogger<AppConfigService> logger)
    {
        // Log only once since Singleton -> single instance shared across app
        logger.LogInformation("AppConfigService (Singleton) instance created");
    }

    // Get current tax rate
    public decimal GetTaxRate()
    {
        return _taxRate;
    }

    // Calculate delivery fee based on order amount
    public decimal GetDeliveryFee(decimal orderAmount)
    {
        if(orderAmount < 500) return 50;
        else if (orderAmount >= 500 && orderAmount <= 2000) return 30;
        else return 0;
    }
}