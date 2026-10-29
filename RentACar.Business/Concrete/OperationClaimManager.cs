using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.OperationClaimDtos;

namespace RentACar.Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;

        public OperationClaimManager(IOperationClaimRepository operationClaimRepository, IMapper mapper)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<OperationClaimResultDto>>> GetAllAsync()
        {
            var operationClaims = await _operationClaimRepository.GetAllAsync();
            var sortedOperationClaims = operationClaims.OrderByDescending(x => x.CreatedDate).ToList();
            var operationClaimDtos = _mapper.Map<List<OperationClaimResultDto>>(sortedOperationClaims);
            return new SuccessDataResult<List<OperationClaimResultDto>>(operationClaimDtos, "Yetkiler başarıyla listelendi.");
        }

        public async Task<IDataResult<OperationClaimResultDto>> GetByIdAsync(int id)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (operationClaim == null)
            {
                throw new BusinessException("Yetki bulunamadı.");
            }

            var operationClaimDto = _mapper.Map<OperationClaimResultDto>(operationClaim);
            return new SuccessDataResult<OperationClaimResultDto>(operationClaimDto, "Yetki başarıyla getirildi.");
        }

        public async Task<IDataResult<int>> GetIdByNameAsync(string name)
        {
            name = name.ToLower().Trim();
            var existingClaim = await _operationClaimRepository.GetAsync(x => x.Name == name);
            if (existingClaim == null) { return new ErrorDataResult<int>("İstenen rütbe bulunamdı!"); }
            return new SuccessDataResult<int>(existingClaim.Id);
        }
    }
}