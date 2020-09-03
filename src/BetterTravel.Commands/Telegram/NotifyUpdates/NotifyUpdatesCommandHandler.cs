using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Entities;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace BetterTravel.Commands.Telegram.NotifyUpdates
{
    public class NotifyUpdatesCommandHandler : CommandHandlerBase<NotifyUpdatesCommand, NotifyUpdatesViewModel>
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITelegramBotClient _telegram;

        public NotifyUpdatesCommandHandler(
            IUserRepository userRepository, 
            ITelegramBotClient telegram)
        {
            _userRepository = userRepository;
            _telegram = telegram;
            _logger = Log.ForContext<NotifyUpdatesCommandHandler>();
        }

        public override async Task<IHandlerResult<NotifyUpdatesViewModel>> Handle(
            NotifyUpdatesCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.Information("NotifyUpdatesCommand invoked.");

            var subscribedUsers = await _userRepository.GetAllAsync(u => u.IsSubscribed, u => u.ChatId);
            subscribedUsers.ForEach(async chatId => await SendTours(chatId, request.Tours, cancellationToken));
            
            return Ok();
        }

        private async Task SendTours(long chatId, List<Tour> tours, CancellationToken cancellationToken)
        {
            foreach (var tour in tours)
            {
                if (tour.ImgUrl != null)
                {
                    var uri = new Uri(tour.ImgUrl);
                    await _telegram.SendPhotoAsync(chatId, new InputOnlineFile(uri), cancellationToken: cancellationToken);
                }

                if (tour.Text != null)
                {
                    await _telegram.SendTextMessageAsync(chatId, tour.Text, cancellationToken: cancellationToken);
                }
            }
        }
    }
}