using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Infrastructure;
using Server.Loan.Infrastructure.Configuration;
using Server.Module.Framework;
using Server.Loan.Application;

namespace Server.Loan.Module;

public class ModuleInstaller : IModuleInstaller
{
    public string Name => nameof(Loan);

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var directory = Path.GetDirectoryName(typeof(ModuleInstaller).Assembly.Location);

        ArgumentException.ThrowIfNullOrEmpty(directory, nameof(directory));

        var currentConfiguration = new ConfigurationBuilder()
         .SetBasePath(directory)
         .AddJsonFile("Loan.json", optional: false, reloadOnChange: true)
         .Build();

        services.Configure<LoanConfiguration>(currentConfiguration.GetSection(nameof(Loan)));

        services.AddApplication();
        services.AddInfrastructure();
    }

    public void Use(IApplicationBuilder host, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
