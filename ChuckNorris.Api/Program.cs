using ChuckNorris.Core.Services;
using ChuckNorris.Api.Extensions;
using ChuckNorris.Core.Services.Transformations;

await using var app = CreateApp(args);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddConnector<IChackNorriesConnector, ChackNorriesConnector>(configuration);
    services.AddTransformation<IJokeTransformation, ReplacementTransformation>(configuration);
    services.AddTransformation<IJokeTransformation, BadWordsFilter>(configuration);
    services.AddScoped<IJokeHandler, JokeHandler>();
}


static WebApplication CreateApp(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    ConfigureServices(builder.Services, builder.Configuration);
 
    return builder.Build();
}
