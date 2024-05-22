using MAST_Service;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MAST_Service.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        
        services.AddSingleton<ICheckOutProcessService, CheckOutProcessService>();
        services.AddDbContext<MastContext>(opts => { }, ServiceLifetime.Singleton);
        services.AddHostedService<MainService>();

    }).UseSerilog()
    .Build();

var configSetting = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(configSetting["Logging:LogPath"]).CreateLogger();

host.Run();
