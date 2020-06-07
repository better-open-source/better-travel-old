using System.Threading.Tasks;
using AutoMapper;
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

            _ = update.Message.Text switch
            {
                "/start" => 0,
                "/subscribe" => 1,
                "/unsubscribe" => 2,
                _ => 3
            };
        }
    }
}