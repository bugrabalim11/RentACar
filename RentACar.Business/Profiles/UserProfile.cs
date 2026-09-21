using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.AuthDtos;
using RentACar.Core.Entities.DTOs.UserDtos;

namespace RentACar.Business.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserUpdateByAdminDto, User>().ReverseMap();
            CreateMap<UserProfileUpdateDto, User>();
            CreateMap<User, UserResultDto>();
            CreateMap<User, UserResultByAdminDto>();

            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserCreateByAdminDto, User>();
        }
    }
}
