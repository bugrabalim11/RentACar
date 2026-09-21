using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;

namespace RentACar.Business.Profiles
{
    public class OperationClaimProfile : Profile
    {
        public OperationClaimProfile()
        {
            CreateMap<OperationClaimCreateDto, OperationClaim>();
            CreateMap<OperationClaimUpdateDto, OperationClaim>();
            CreateMap<OperationClaim, OperationClaimResultDto>();
        }
    }
}
