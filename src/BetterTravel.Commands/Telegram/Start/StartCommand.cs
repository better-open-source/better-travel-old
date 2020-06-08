using BetterTravel.Commands.Abstraction;

namespace BetterTravel.Commands.Telegram.Start
{
    public class StartCommand : ICommand<StartViewModel>
    {
        public long ChatId { get; set; }
    }
}