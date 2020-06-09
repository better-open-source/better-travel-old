using AutoMapper;
using BetterTravel.Commands.Telegram.Start;
using BetterTravel.DataAccess.Abstractions.Entities;

namespace BetterTravel.API.Requests.Mappings
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<StartCommand, User>();
        }
    }
}