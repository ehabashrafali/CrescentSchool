using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CrescentSchool.Core.Extensions;

public static class ServiceCollectionExtensions
{
    //
    // Summary:
    //     Retrieves an instance of the specified service from the service collection.
    //
    // Parameters:
    //   services:
    //     The service collection.
    //
    // Type parameters:
    //   TService:
    //     The type of service to retrieve.
    //
    // Returns:
    //     An instance of the specified service if found, or null if not found.
    public static TService? GetService<TService>(this IServiceCollection services) where TService : notnull
    {
        return services.BuildServiceProvider().GetService<TService>();
    }

    //
    // Summary:
    //     Retrieves an instance of the specified service from the service collection.
    //
    // Parameters:
    //   services:
    //     The service collection.
    //
    // Type parameters:
    //   TService:
    //     The type of service to retrieve.
    //
    // Returns:
    //     An instance of the specified service if found, or null if not found.
    public static TService GetRequiredService<TService>(this IServiceCollection services) where TService : notnull
    {
        return services.BuildServiceProvider().GetRequiredService<TService>();
    }

    //
    // Summary:
    //     Gets the options of type TOptions.
    //
    // Parameters:
    //   services:
    //     The Microsoft.Extensions.DependencyInjection.IServiceCollection instance.
    //
    // Type parameters:
    //   TOptions:
    //     The type of options to retrieve.
    //
    // Returns:
    //     The options of type TOptions.
    public static TOptions GetOptions<TOptions>(this IServiceCollection services) where TOptions : class
    {
        return services.GetRequiredService<IOptions<TOptions>>().Value;
    }

    //
    // Summary:
    //     Gets the options of type TOptions.
    //
    // Parameters:
    //   services:
    //     The service collection.
    //
    // Type parameters:
    //   TOptions:
    //     The type of options to retrieve.
    //
    // Returns:
    //     The options instance of the specified type.
    public static TOptions GetOptions<TOptions>(this IServiceProvider services) where TOptions : class
    {
        return services.GetRequiredService<IOptions<TOptions>>().Value;
    }

    //
    // Summary:
    //     Binds an options object to a configuration section and adds validation for data
    //     annotations.
    //
    // Parameters:
    //   services:
    //     The Microsoft.Extensions.DependencyInjection.IServiceCollection instance.
    //
    //   sectionName:
    //     The name of the configuration section to bind the options object to.
    //
    // Type parameters:
    //   TOptions:
    //     The type of options to bind.
    //
    // Returns:
    //     The updated Microsoft.Extensions.DependencyInjection.IServiceCollection instance.
    public static IServiceCollection BindOptions<TOptions>(this IServiceCollection services, string sectionName) where TOptions : class
    {
        services.AddOptions<TOptions>().BindConfiguration(sectionName).ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }
}