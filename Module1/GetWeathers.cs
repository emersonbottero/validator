// Ignore Spelling: Validator

using Mediator;

namespace Module1;

public class GetWeathers : IQuery<WeatherForecast[]?>
{
    public string City { get; set; }
}

public class GetWeatherValidator : Validator<GetWeathers>
{
    public GetWeatherValidator()
    {
        Should(g => g.City.Length > 3, "City must have at least 3 letters");
    }
}

public class GetWeathersandler : IQueryHandler<GetWeathers, WeatherForecast[]?>
{

    async ValueTask<WeatherForecast[]?> IQueryHandler<GetWeathers, WeatherForecast[]?>.Handle(GetWeathers query, CancellationToken cancellationToken)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
        return forecast;
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}