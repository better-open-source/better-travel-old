using AutoMapper;
using BetterTravel.Commands.Telegram.Subscribe;
using BetterTravel.Commands.Telegram.Unsubscribe;
using Telegram.Bot.Types;

namespace BetterTravel.API.Requests.Mappings
{
    public class TelegramProfile : Profile
    {
        public TelegramProfile()
        {
            CreateMap<Update, UnsubscribeCommand>()
                .ForMember(m => m.ChatId, exp => exp.MapFrom(f => f.Message.Chat.Id));

            CreateMap<Update, SubscribeCommand>()
                .ForMember(m => m.ChatId, exp => exp.MapFrom(f => f.Message.Chat.Id));
        }
    }
}