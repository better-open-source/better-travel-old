using BetterTravel.Commands.Abstraction;

namespace BetterTravel.Commands.Telegram.Start
{
    public class StartCommand : ICommand<StartViewModel>
    {
        public long ChatId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsBot { get; set; }
    }
}