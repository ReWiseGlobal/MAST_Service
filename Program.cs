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
    .ConfigureServices((hostContext,services) =>
    {

        services.AddDbContext<MastContext>(opts =>
                    opts.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")),
                    ServiceLifetime.Scoped);

        services.AddScoped<ICheckOutProcessService, CheckOutProcessService>();

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
