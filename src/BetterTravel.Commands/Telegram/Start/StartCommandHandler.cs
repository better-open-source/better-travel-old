using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Entities;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using BetterTravel.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = BetterTravel.DataAccess.Abstractions.Entities.User;

namespace BetterTravel.Commands.Telegram.Start
{
    public class StartCommandHandler : CommandHandlerBase<StartCommand, StartViewModel>
    {
        private readonly ITelegramBotClient _telegram;
        private readonly ILogger _logger;
        private readonly IToursFetcherService _toursFetcher;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public StartCommandHandler(
            ITelegramBotClient telegram, 
            IToursFetcherService toursFetcher, 
            IUserRepository userRepository, 
            IMapper mapper)
        {
            _logger = Log.ForContext<StartCommandHandler>();
            _telegram = telegram;
            _toursFetcher = toursFetcher;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task<IHandlerResult<StartViewModel>> Handle(
            StartCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.Information("StartCommand invoked.");

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

            var dbUser = await _userRepository.GetFirstAsync(u => u.ChatId == request.ChatId);
            if (!(dbUser is null))
            {
                var message = dbUser.IsSubscribed
                    ? "were"
                    : "were not";
                return ValidationFailed(
                    $"You already registered to the bot previously and {message} subscribed to updates.");
            }

            var newUser = _mapper.Map<User>(request);
            newUser.IsSubscribed = true;
            newUser.RegisteredAt = DateTime.Now;

            await _userRepository.CreateAsync(newUser);
            
            var tours = await _toursFetcher.FetchToursAsync(true, 10);
            
            foreach (var tour in tours)
            {
                if (tour.ImgUrl != null)
                {
                    var uri = new Uri(tour.ImgUrl);
                    await _telegram.SendPhotoAsync(request.ChatId, new InputOnlineFile(uri), cancellationToken: cancellationToken);
                }

                if (tour.Text != null)
                {
                    await _telegram.SendTextMessageAsync(request.ChatId, tour.Text, cancellationToken: cancellationToken);
                }
            }
            
            return Ok();
        }
    }
}