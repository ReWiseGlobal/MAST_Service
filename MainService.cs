﻿using Microsoft.Extensions.Configuration;
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
        private readonly ICheckOutProcessService checkOutProcess;
        
        public MainService(ILogger<MainService> _logger, ICheckOutProcessService checkOut)
        {
            this.logger = _logger;
            checkOutProcess = checkOut;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    checkOutProcess.checkExecutorProcess();
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
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
