using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;

namespace RentACar.Business.Profiles
{
    public class UserOperationClaimProfile : Profile
    {
        public UserOperationClaimProfile()
        {
            CreateMap<UserOperationClaimCreateDto, UserOperationClaim>();
            CreateMap<UserOperationClaimUpdateDto, UserOperationClaim>().ReverseMap();
            CreateMap<UserOperationClaim, UserOperationClaimResultDto>();
        }
    }
}
