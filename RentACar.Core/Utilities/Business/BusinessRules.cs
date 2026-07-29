using RentACar.Core.Utilities.Results;

namespace RentACar.Core.Utilities.Business
{
    // Static yapıyoruz çünkü bu asistana her dükkandan anında ulaşabilmeliyiz, new'leyerek uğraşmamalıyız
    public static class BusinessRules
    {
        /// <summary>
        /// Dükkan müdürlerinin (Manager) iş kurallarını çalıştıran motordur.
        /// Verilen kuralları (logics) sırayla kontrol eder. 
        /// Eğer kural patlarsa (Success == false), anında o hatayı müdüre geri fırlatır.
        /// </summary>
        /// <param name="logics">Kontrol edilecek iş kuralları (params ile sınırsız sayıda kural gönderilebilir)</param>
        /// <returns>Kural ihlali varsa ErrorResult (Hata), her şey sorunsuzsa null (Temiz) döner.</returns>
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
