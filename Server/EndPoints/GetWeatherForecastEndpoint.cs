using FastEndpoints;

namespace Server.EndPoints;

public class GetWeatherForecastEndpoint : EndpointWithoutRequest<IEnumerable<WeatherForecast>>
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public override void Configure()
    {
        Get("/weatherforecast");
        AllowAnonymous();
        Description(b => b
            .WithName("GetWeatherForecast")
            .Produces<IEnumerable<WeatherForecast>>(200));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        await SendAsync(forecast, cancellation: ct);
    }
}
