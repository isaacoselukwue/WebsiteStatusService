using WebsiteStatusApp;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    }).UseSerilog()
    .Build();


Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().MinimumLevel
    .Override("Microsoft",Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext().WriteTo.File(@"C:\workerservice\LogFile.txt")
    .CreateLogger();

try
{
    Log.Information("Starting up theservice");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service");
    return;

}
finally
{
    Log.CloseAndFlush();
}