using AutoMapper;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class UserOperationClaimProfile : Profile
    {
        public UserOperationClaimProfile()
        {
            CreateMap<UserOperationClaimCreateDto, UserOperationClaim>();
            CreateMap<UserOperationClaimUpdateDto, UserOperationClaim>();
            CreateMap<UserOperationClaim, UserOperationClaimResultDto>();
        }
    }
}
