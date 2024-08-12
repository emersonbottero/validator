using Mediator;
using Module1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediator();

builder.Services.AddTransient<IValidator<GetWeathers>, GetWeatherValidator>();

builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast/{city}", async (IMediator mediator, string city) =>
{
    return await mediator.Send(new GetWeathers() { City = city });
})
.AddEndpointFilter<ValidationExceptionFilter>()
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


