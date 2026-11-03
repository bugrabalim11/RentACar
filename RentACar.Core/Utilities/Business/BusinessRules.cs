using RentACar.Core.Utilities.Results;

namespace RentACar.Core.Utilities.Business
{
    /// <summary>
    /// İş kurallarını (Business Rules) merkezi olarak denetleyen motordur.
    /// Parametre olarak verilen kuralları sırayla kontrol eder ve başarısız olan ilk kuralın sonucunu (ErrorResult) döner.
    /// Tüm kurallar başarılıysa null döner.
    /// </summary>
    public static class BusinessRules
    {
        public static IResult? Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }
            return null;
        }
    }
}
