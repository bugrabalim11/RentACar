using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Dtos.AuthDtos;
using RentACar.Dtos.UserDtos;

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
