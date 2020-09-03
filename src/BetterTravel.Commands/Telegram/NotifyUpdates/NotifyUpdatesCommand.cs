using System.Collections.Generic;
using BetterTravel.Commands.Abstraction;
using BetterTravel.DataAccess.Abstractions.Entities;

namespace BetterTravel.Commands.Telegram.NotifyUpdates
{
    public class NotifyUpdatesCommand : ICommand<NotifyUpdatesViewModel>
    {
        public List<Tour> Tours { get; set; }
    }
}