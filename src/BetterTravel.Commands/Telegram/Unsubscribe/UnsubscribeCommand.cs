using BetterTravel.Commands.Abstraction;

namespace BetterTravel.Commands.Telegram.Unsubscribe
{
    public class UnsubscribeCommand : ICommand<UnsubscribeViewModel>
    {
        public long ChatId { get; set; }
    }
}