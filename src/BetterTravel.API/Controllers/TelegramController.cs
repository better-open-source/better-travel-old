using System.Threading.Tasks;
using AutoMapper;
using BetterTravel.Commands.Telegram.Start;
using BetterTravel.Commands.Telegram.Status;
using BetterTravel.Commands.Telegram.Subscribe;
using BetterTravel.Commands.Telegram.Unsubscribe;
using BetterTravel.MediatR.Core.HandlerResults;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BetterTravel.API.Controllers
{
    [Route("api/[controller]")]
    public class TelegramController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ITelegramBotClient _telegram;

        public TelegramController(IMediator mediator, IMapper mapper, ITelegramBotClient telegram)
        {
            _mediator = mediator;
            _mapper = mapper;
            _telegram = telegram;
            _logger = Log.ForContext<TelegramController>();
        }

        [HttpPost("update/{token}")]
        public async Task Update([FromRoute] string token, [FromBody] Update update)
        {
            _logger.Information("We received new {@message}", update);
            if (update is null)
            {
                _logger.Error("Update is null");
                return;
            }

            switch (update.Message.Text)
            {
                case "/start":
                    var start = _mapper.Map<StartCommand>(update);
                    var startResult = await _mediator.Send(start);
                    await SendResult(update.Message.Chat.Id, startResult);
                    break;
                case "/subscribe":
                    var subscribe = _mapper.Map<SubscribeCommand>(update);
                    var subscribeResult = await _mediator.Send(subscribe);
                    await SendResult(update.Message.Chat.Id, subscribeResult);
                    break;
                case "/unsubscribe":
                    var unsubscribe = _mapper.Map<UnsubscribeCommand>(update);
                    var unsubscribeResult = await _mediator.Send(unsubscribe);
                    await SendResult(update.Message.Chat.Id, unsubscribeResult);
                    break;
                case "/status":
                    var status = _mapper.Map<StatusCommand>(update);
                    var statusResult = await _mediator.Send(status);
                    await SendResult(update.Message.Chat.Id, statusResult);
                    break;
                default:
                    _ = 3;
                    break;
            }
        }

        private async Task SendResult<T>(long chatId, IHandlerResult<T> result)
        {
            switch (result)
            {
                case ValidationFailedHandlerResult<T> validationFailedHandlerResult:
                    await _telegram.SendTextMessageAsync(chatId, validationFailedHandlerResult.Message);
                    break;
            }
        }
    }
}