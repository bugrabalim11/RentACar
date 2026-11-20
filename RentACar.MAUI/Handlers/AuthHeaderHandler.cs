using System.Net.Http.Headers;

namespace RentACar.MAUI.Handlers
{
    /// <summary>
    /// API'ye giden her HTTP isteğini yakalayıp, SecureStorage'teki JWT Token'ı Authorization başlığına ekleyen ara katman.
    /// </summary>
    public class AuthHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await SecureStorage.Default.GetAsync("AuthToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
