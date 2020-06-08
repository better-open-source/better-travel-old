using BetterTravel.Commands.Abstraction;

namespace BetterTravel.Commands.Telegram.Subscribe
{
    public class SubscribeCommand : ICommand<SubscribeViewModel>
    {
        public long ChatId { get; set; }
    }
}