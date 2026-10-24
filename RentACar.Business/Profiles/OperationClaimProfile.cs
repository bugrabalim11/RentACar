using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Dtos.OperationClaimDtos;

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
