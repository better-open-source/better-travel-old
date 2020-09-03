using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BetterTravel.Commands.Telegram.Subscribe
{
    public class SubscribeCommandHandler : CommandHandlerBase<SubscribeCommand, SubscribeViewModel>
    {        
        private const string SubscribedMessage = "Subscribed to updates.";

        private readonly ITelegramBotClient _telegram;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public SubscribeCommandHandler(
            ITelegramBotClient telegram, 
            IUserRepository userRepository)
        {
            _telegram = telegram;
            _userRepository = userRepository;
            _logger = Log.ForContext<SubscribeCommandHandler>();
        }

        public override async Task<IHandlerResult<SubscribeViewModel>> Handle(
            SubscribeCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.Information("SubscribeCommand invoked.");
            
            if (request.IsBot)
            {
                _logger.Fatal("Bot attempted to subscribe!");
                return ValidationFailed("Sorry, no bots here!");
            }

            if (request.ChatId <= 0)
            {
                _logger.Warning($"Bad parameter: {nameof(ChatId)}!");
                return ValidationFailed($"Bad parameter: {nameof(ChatId)}!");
            }

            var user = await _userRepository.GetFirstAsync(u => u.ChatId == request.ChatId);
            
            if (user is null)
                return ValidationFailed("Invalid user.");

            if (user.IsSubscribed)
                return ValidationFailed("You already subscribed to updates.");

            await _userRepository.SubscribeAsync(request.ChatId);
            
            await _telegram.SendTextMessageAsync(request.ChatId, SubscribedMessage,
                cancellationToken: cancellationToken);

            return Ok();
        }
    }
}