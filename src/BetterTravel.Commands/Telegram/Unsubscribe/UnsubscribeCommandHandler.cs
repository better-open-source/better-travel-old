using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;

namespace BetterTravel.Commands.Telegram.Unsubscribe
{
    public class UnsubscribeCommandHandler : CommandHandlerBase<UnsubscribeCommand, UnsubscribeViewModel>
    {
        private readonly ITelegramBotClient _telegram;
        private readonly ILogger _logger;

        public UnsubscribeCommandHandler(ITelegramBotClient telegram)
        {
            _telegram = telegram;
            _logger = Log.ForContext<UnsubscribeCommandHandler>();
        }

        public override Task<IHandlerResult<UnsubscribeViewModel>> Handle(
            UnsubscribeCommand request, 
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}