using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using BetterTravel.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BetterTravel.Commands.Telegram.Start
{
    public class StartCommandHandler : CommandHandlerBase<StartCommand, StartViewModel>
    {
        private readonly ITelegramBotClient _telegram;
        private readonly ILogger _logger;
        private readonly IToursFetcherService _toursFetcher;
        private readonly IUserRepository _userRepository;

        public StartCommandHandler(
            ITelegramBotClient telegram, 
            IToursFetcherService toursFetcher, 
            IUserRepository userRepository)
        {
            _logger = Log.ForContext<StartCommandHandler>();
            _telegram = telegram;
            _toursFetcher = toursFetcher;
            _userRepository = userRepository;
        }

        public override async Task<IHandlerResult<StartViewModel>> Handle(
            StartCommand request, 
            CancellationToken cancellationToken)
        {
            if (request.ChatId <= 0)
            {
                _logger.Warning($"Bad parameter: {nameof(ChatId)}!");
                return ValidationFailed($"Bad parameter: {nameof(ChatId)}!");
            }
            
            
            
            var tours = await _toursFetcher.FetchToursAsync(true, 10);
            
            throw new System.NotImplementedException();
        }
    }
}