using System.Text.RegularExpressions;

namespace RentACar.API.Extensions
{
    /// <summary>
    /// API rotalarındaki (URL) PascalCase kelime geçişlerini RESTful standartlarına uygun olarak
    /// kebab-case formatına çeviren yönlendirme kuralı dönüştürücüsüdür.
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) { return null; }

            string str = value.ToString()!;

            string kebabCase = Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLower();

            return kebabCase.ToLower();
        }
    }
}
