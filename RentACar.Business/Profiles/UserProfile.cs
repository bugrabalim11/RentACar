using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserListDto>();

            CreateMap<UserForRegisterDto, User>();
        }
    }
}
