using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Module.Framework;
using Server.Products.Infrastructure;
using Server.Products.Infrastructure.Configuration;

namespace Server.Products.Module
{
    public class ModuleInstaller : IModuleInstaller
    {
        public string Name => nameof(Products);

        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            var directory = Path.GetDirectoryName(typeof(ModuleInstaller).Assembly.Location);

            ArgumentException.ThrowIfNullOrEmpty(directory, nameof(directory));

            var currentConfiguration = new ConfigurationBuilder()
             .SetBasePath(directory)
             .AddJsonFile("Products.json", optional: false, reloadOnChange: true)
             .Build();

            services.Configure<ProductsConfiguration>(currentConfiguration.GetSection(nameof(Products)));


            services.AddInfrastructure();
        }

        public void Use(IApplicationBuilder host, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
