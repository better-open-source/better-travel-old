using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BetterTravel.Commands.Telegram.Start
{
    public class StartCommandHandler : CommandHandlerBase<StartCommand, StartViewModel>
    {
        private readonly ITelegramBotClient _telegram;
        private ILogger _logger;

        public StartCommandHandler(ITelegramBotClient telegram)
        {
            _logger = Log.ForContext<StartCommandHandler>();
            _telegram = telegram;
        }

        public override Task<IHandlerResult<StartViewModel>> Handle(
            StartCommand request, 
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}