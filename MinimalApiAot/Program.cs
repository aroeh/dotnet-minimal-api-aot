using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MinimalApiAot.DataAccess;
using MinimalApiAot.Health;
using MinimalApiAot.HttpClientHelpers;
using MinimalApiAot.Middleware;
using MinimalApiAot.Models;
using MinimalApiAot.Repos;
using MinimalApiAot.RouteExtensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Add and include additional app settings files
// by default
// builder.Configuration
//     .AddJsonFile("appsettings.json")
//     .AddJsonFile("appsettings.Development.json");

// Add services to the container.

// register a custom exception handler middleware and problem details usage in the pipeline
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add API versioning services
builder.Services.AddApiVersioning();

// configure authoriation and authentication for the API with Microsoft EntraID
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// add health checks to the app
// this is currently using a custom mongodb health check
builder.Services.AddHealthChecks()
    .AddCheck<CustomMongoDbHealthCheck>("Database");

// setup API output cache policy to use on specified controllers
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromSeconds(60)));
});

// register http clients for the app
// add a typed HttpClient
builder.Services.AddHttpClient<HttpClientHelper>(c => c.BaseAddress = new Uri("http://localhost:5219"));

// add a named IHttpClientFactory
builder.Services.AddHttpClient("restuarantClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5219");
});
builder.Services.AddTransient<HttpFactoryHelper>();


// Add classes and interfaces for dependency injection
// transient is being used here so new instances of classes are insantiated when needed in the request pipeline
builder.Services.AddTransient<IMongoService, MongoService>();
builder.Services.AddTransient<IRestuarantRepo, RestuarantRepo>();
builder.Services.AddTransient<IRestuarantData, RestuarantData>();

// add hsts security headers
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var app = builder.Build();

// Sets up a test endpoint for the API which uses one of the http client instances
// This is just an example of setting an endpoint within program.cs
// The content of this endpoint is far from ideal, and again is just showing a possibility
app.MapGet("/", () => "Welcome to the Reference .Net Minimal API using AOT");
var testApi = app.MapGroup("/test");
testApi.MapGet("/", async (HttpFactoryHelper httpHelper) =>
{
    Restuarant addRestuarant = new()
    {
        Name = "Test",
        CuisineType = "Test",
        Website = new Uri("https://www.google.com/"),
        Phone = "1112223333",
        Address = new()
        {
            Street = "123 Test Street",
            City = "Somewhere",
            State = "KY",
            ZipCode = "12345",
            Country = "United States"
        }
    };

    bool addResult = await httpHelper.PostAsync<Restuarant, bool>("/restuarant/v2", addRestuarant);

    List<Restuarant> restuarants = await httpHelper.GetAsync<List<Restuarant>>("/restuarant/v2");

    return Results.Ok(restuarants);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// use the global exception handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

// map the health check endpoint and setup a custom writer
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckResponseWriter.WriteCustomHealthCheckResponse
});

app.UseOutputCache();

// map custom route extensions - can help keep the program.cs file less cluttered
app.MapRestuarantV1Routes();
app.MapRestuarantV2Routes();

app.Run();


// Specific to the AOT project template, setup object types for the Json Serialization on both input and output from endpoints
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(Restuarant))]
[JsonSerializable(typeof(Restuarant[]))]
[JsonSerializable(typeof(List<Restuarant>))]
[JsonSerializable(typeof(SearchCriteria))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
