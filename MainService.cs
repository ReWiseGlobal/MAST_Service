using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MAST_Service
{
    public class MainService : BackgroundService
    {
        private readonly ILogger<MainService> logger;
        //private readonly ICheckOutProcessService checkOutProcess;
        private readonly IReleaseDateReminderService releaseDateReminderService;


        private readonly IServiceProvider _serviceProvider;

        public MainService(IServiceProvider serviceProvider, ILogger<MainService> _logger, IReleaseDateReminderService _releaseDateReminderService)
        {
            _serviceProvider = serviceProvider;
            this.logger = _logger;
            releaseDateReminderService = _releaseDateReminderService;


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var checkOutProcessService = scope.ServiceProvider.GetRequiredService<ICheckOutProcessService>();
                    checkOutProcessService.checkExecutorProcess();
                    // Perform operations using checkOutProcessService

                    releaseDateReminderService.checkEmployeeReleaseDate();
                }

                await Task.Delay(1000 * 60, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service started...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service stopped...");
            return base.StopAsync(cancellationToken);
        }
    }
}
