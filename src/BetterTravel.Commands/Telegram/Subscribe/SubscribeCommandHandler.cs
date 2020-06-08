using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;

namespace BetterTravel.Commands.Telegram.Subscribe
{
    public class SubscribeCommandHandler : CommandHandlerBase<SubscribeCommand, SubscribeViewModel>
    {
        private readonly ITelegramBotClient _telegram;
        private ILogger _logger;

        public SubscribeCommandHandler(ITelegramBotClient telegram)
        {
            _telegram = telegram;
            _logger = Log.ForContext<SubscribeCommandHandler>();
        }

        public override Task<IHandlerResult<SubscribeViewModel>> Handle(
            SubscribeCommand request, 
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}