
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Data.Interceptors;
using LibraryManagementAPI.Services;
using LibraryManagementAPI.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Extensions;

public static class ServiceCollectionExtension
{

    // Database
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Interceptors
        services.AddScoped<AuditInterceptor>();
        
        // Database
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            )
            .AddInterceptors(
                serviceProvider.GetRequiredService<AuditInterceptor>()
            )            
        );

        return services;
    }


    // UnitOfWork
    public static IServiceCollection AddUnitWork(
        this IServiceCollection services
    )
    {
        services.AddScoped<UnitOfWork>();
        return services;
    }

    // Services
    public static IServiceCollection AddServices(
        this IServiceCollection services
    )
    {

        // The bug is a circular dependency introduced by the interceptor DI setup.
        // AppDbContext -> AuditInterceptor -> AuditLogService -> UnitOfWork -> AppDbContext


        // services.AddScoped<AuditLogService>();
        services.AddScoped<BookService>();
        return services;
    }
}