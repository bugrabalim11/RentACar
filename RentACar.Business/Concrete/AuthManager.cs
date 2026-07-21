using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        public Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UserExist(string email)
        {
            throw new NotImplementedException();
        }
    }
}
