using TradeGenius.WebApi.Application;
using TradeGenius.WebApi.Host.Configurations;
using TradeGenius.WebApi.Host.Controllers;
using TradeGenius.WebApi.Infrastructure;
using TradeGenius.WebApi.Infrastructure.Common;
using TradeGenius.WebApi.Infrastructure.Logging.Serilog;
using Serilog;

[assembly: ApiConventionType(typeof(TradeGeniusApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);

    await app.Services.InitializeRecurringJobsAsync();

    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}