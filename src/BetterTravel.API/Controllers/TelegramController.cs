using System.Threading.Tasks;
using AutoMapper;
using BetterTravel.Commands.Telegram.Unsubscribe;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Telegram.Bot.Types;

namespace BetterTravel.API.Controllers
{
    [Route("api/[controller]")]
    public class TelegramController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TelegramController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
                    _ = 0;
                    break;
                case "/subscribe":
                    _ = 1;
                    break;
                case "/unsubscribe":
                    var command = _mapper.Map<UnsubscribeCommand>(update);
                    var result = await _mediator.Send(command);
                    break;
                default:
                    _ = 3;
                    break;
            }
        }
    }
}