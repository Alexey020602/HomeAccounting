
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Tests;

interface IA;

interface IB;

class Implementation : IA, IB;

public class ServiceCollectionTests
{
    [Fact]
    public void ScopedWithScopedImplementations()
    {
        var services = new ServiceCollection();

        services.AddScoped<Implementation>();
        services.AddScoped<IA>(sp => sp.GetRequiredService<Implementation>());
        services.AddScoped<IB>(sp => sp.GetRequiredService<Implementation>());
        
        var serviceProvider = services.BuildServiceProvider();
        using var firstScope = serviceProvider.CreateScope();
        
        var aFirstScope = firstScope.ServiceProvider.GetRequiredService<IA>();
        var aFirstScope2 = firstScope.ServiceProvider.GetRequiredService<IA>();
        var bFirstScope = firstScope.ServiceProvider.GetRequiredService<IB>();
        
        using var secondScope = firstScope.ServiceProvider.CreateScope();
        
        var aSecondScope = secondScope.ServiceProvider.GetRequiredService<IA>();
        var bSecondScope = secondScope.ServiceProvider.GetRequiredService<IB>();
        
        Assert.Same(aFirstScope, aFirstScope2);
        Assert.Same(aFirstScope, bFirstScope);
        Assert.Same(aSecondScope, bSecondScope);
        Assert.NotSame(aFirstScope, bSecondScope);
    }
    
    [Fact]
    public void ScopedWithTransientImplementations()
    {
        var services = new ServiceCollection();

        services.AddScoped<Implementation>();
        services.AddTransient<IA>(sp => sp.GetRequiredService<Implementation>());
        services.AddTransient<IB>(sp => sp.GetRequiredService<Implementation>());
        
        var serviceProvider = services.BuildServiceProvider();
        using var firstScope = serviceProvider.CreateScope();
        
        var aFirstScope = firstScope.ServiceProvider.GetRequiredService<IA>();
        var aFirstScope2 = firstScope.ServiceProvider.GetRequiredService<IA>();
        var bFirstScope = firstScope.ServiceProvider.GetRequiredService<IB>();
        
        using var secondScope = firstScope.ServiceProvider.CreateScope();
        
        var aSecondScope = secondScope.ServiceProvider.GetRequiredService<IA>();
        var bSecondScope = secondScope.ServiceProvider.GetRequiredService<IB>();
        
        Assert.Same(aFirstScope, aFirstScope2);
        Assert.Same(aFirstScope, bFirstScope);
        Assert.Same(aSecondScope, bSecondScope);
        Assert.NotSame(aFirstScope, bSecondScope);
    }
}