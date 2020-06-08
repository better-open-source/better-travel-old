using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BetterTravel.Commands.Telegram.Status
{
    public class StatusCommandHandler : CommandHandlerBase<StatusCommand, StatusViewModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITelegramBotClient _telegram;
        private readonly ILogger _logger;

        public StatusCommandHandler(
            IUserRepository userRepository, 
            ITelegramBotClient telegram)
        {
            _userRepository = userRepository;
            _telegram = telegram;
            _logger = Log.ForContext<StatusCommandHandler>();
        }

        public override async Task<IHandlerResult<StatusViewModel>> Handle(
            StatusCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.Information("StatusCommand invoked.");
            
            if (request.ChatId <= 0)
            {
                _logger.Warning($"Bad parameter: {nameof(ChatId)}!");
                return ValidationFailed($"Bad parameter: {nameof(ChatId)}!");
            }

            var user = await _userRepository.GetFirstAsync(u => u.ChatId == request.ChatId);
            if (user is null)
                return ValidationFailed("Was not able to find any info about you. Seems like you doesn't exist!");

            var message = user.IsSubscribed
                ? "You are subscribed."
                : "You are not subscribed.";
            
            await _telegram.SendTextMessageAsync(request.ChatId, message, cancellationToken: cancellationToken);
            
            return Ok();
        }
    }
}