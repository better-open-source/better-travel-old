using AutoMapper;
using BetterTravel.Commands.Telegram.Start;
using BetterTravel.Commands.Telegram.Status;
using BetterTravel.Commands.Telegram.Subscribe;
using BetterTravel.Commands.Telegram.Unsubscribe;
using Telegram.Bot.Types;

namespace BetterTravel.API.Requests.Mappings
{
    public class TelegramProfile : Profile
    {
        public TelegramProfile()
        {
            CreateMap<Update, StartCommand>()
                .ForMember(m => m.ChatId, exp => exp.MapFrom(f => f.Message.Chat.Id))
                .ForMember(m => m.Username, exp => exp.MapFrom(f => f.Message.From.Username))
                .ForMember(m => m.FirstName, exp => exp.MapFrom(f => f.Message.From.FirstName))
                .ForMember(m => m.LastName, exp => exp.MapFrom(f => f.Message.From.LastName))
                .ForMember(m => m.LanguageCode, exp => exp.MapFrom(f => f.Message.From.LanguageCode))
                .ForMember(m => m.IsBot, exp => exp.MapFrom(f => f.Message.From.IsBot));
            
            CreateMap<Update, StatusCommand>()
                .ForMember(m => m.ChatId, exp => exp.MapFrom(f => f.Message.Chat.Id));
            
            CreateMap<Update, SubscribeCommand>()
                .ForMember(m => m.ChatId, exp => exp.MapFrom(f => f.Message.Chat.Id))
                .ForMember(m => m.IsBot, exp => exp.MapFrom(f => f.Message.From.IsBot));;
            
            CreateMap<Update, UnsubscribeCommand>()
                .ForMember(m => m.ChatId, exp => exp.MapFrom(f => f.Message.Chat.Id));
        }
    }
}