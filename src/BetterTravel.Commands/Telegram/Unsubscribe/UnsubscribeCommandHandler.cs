using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BetterTravel.Commands.Telegram.Unsubscribe
{
    public class UnsubscribeCommandHandler : CommandHandlerBase<UnsubscribeCommand, UnsubscribeViewModel>
    {
        private const string UnsubscribedMessage = "Unsubscribed from updates.";
        
        private readonly ITelegramBotClient _telegram;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public UnsubscribeCommandHandler(
            ITelegramBotClient telegram, 
            IUserRepository userRepository)
        {
            _telegram = telegram;
            _userRepository = userRepository;
            _logger = Log.ForContext<UnsubscribeCommandHandler>();
        }

        public override async Task<IHandlerResult<UnsubscribeViewModel>> Handle(
            UnsubscribeCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.Information("UnsubscribeCommand invoked.");
            
            if (request.ChatId <= 0)
            {
                _logger.Warning($"Bad parameter: {nameof(ChatId)}!");
                return ValidationFailed($"Bad parameter: {nameof(ChatId)}!");
            }

            var user = await _userRepository.GetFirstAsync(u => u.ChatId == request.ChatId);
            
            if (user is null)
                return ValidationFailed("Invalid user.");

            if (!user.IsSubscribed)
                return ValidationFailed("You are not subscribed to updates yet.");

            await _userRepository.UnsubscribeAsync(request.ChatId);

            await _telegram.SendTextMessageAsync(request.ChatId, UnsubscribedMessage,
                cancellationToken: cancellationToken);

            return Ok();
        }
    }
}