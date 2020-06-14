using System;
using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Telegram.NotifyUpdates;
using BetterTravel.Services;
using MediatR;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BetterTravel.API.HostedServices
{
    public class ToursNotifierHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger _logger;
        private readonly IToursFetcherService _toursFetcherService;
        private readonly IMediator _mediator;

        public ToursNotifierHostedService(IToursFetcherService toursFetcherService, IMediator mediator)
        {
            _toursFetcherService = toursFetcherService;
            _mediator = mediator;
            _logger = Log.ForContext<ToursNotifierHostedService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"{nameof(ToursNotifierHostedService)} running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                var tours = await _toursFetcherService.FetchToursAsync(false, 10);
                var command = new NotifyUpdatesCommand {Tours = tours};
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{nameof(ToursNotifierHostedService)} failed.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"{nameof(ToursNotifierHostedService)} is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => 
            _timer?.Dispose();
    }
}