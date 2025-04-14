using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Server.Extensions;

internal static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddOpenTelemetry()
          .WithMetrics(metrics =>
          {
              metrics.AddRuntimeInstrumentation()
                  .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel", "System.Net.Http");
          })
          .WithTracing(tracing =>
          {
              tracing.AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation();
          });

        var useOtlpExporter = !string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        if (useOtlpExporter)
        {
            services.AddOpenTelemetry().UseOtlpExporter();
        }

        return services;
    }
}
