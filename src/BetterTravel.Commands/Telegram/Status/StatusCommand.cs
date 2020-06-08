using BetterTravel.Commands.Abstraction;

namespace BetterTravel.Commands.Telegram.Status
{
    public class StatusCommand : ICommand<StatusViewModel>
    {
        public long ChatId { get; set; }
    }
}