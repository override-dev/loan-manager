using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Module.Framework;

public interface IModuleInstaller
{
    public string Name { get; }
    void Install(IServiceCollection services, IConfiguration configuration);
    void Use(IApplicationBuilder host, IConfiguration configuration);
}
