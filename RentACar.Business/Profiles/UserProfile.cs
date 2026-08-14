using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.AuthDtos;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserUpdateForAdminDto, User>();
            CreateMap<UserProfileUpdateDto, User>();
            CreateMap<User, UserListDto>();
            CreateMap<User, UserListForAdminDto>();

            CreateMap<UserForRegisterDto, User>();
        }
    }
}
