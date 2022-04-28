using AutoMapper;
using MessageApp.Application.ViewModels;
using MessageApp.Domain.Entities.Concrete;

namespace MessageApp.API
{
    public class Profil:Profile
    {
        public Profil()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<MessagePrivate, PrivateMessageViewModel>().ReverseMap();
            CreateMap<MessageGroup, GroupMessageViewModel>().ReverseMap();
            CreateMap<Room, RoomViewModel>().ReverseMap();
        }
        
    }
}
