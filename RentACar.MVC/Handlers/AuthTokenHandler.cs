using System.Net.Http.Headers;

namespace RentACar.MVC.Handlers
{
    /// <summary>
    /// MVC uygulamasından API'ye giden tüm HttpClient HTTP isteklerinin arasına girerek (Interception),
    /// eğer kullanıcı giriş yapmışsa (Cookie'de AccessToken varsa) bu token'ı Authorization Bearer Header olarak isteğe otomatik ekler.
    /// </summary>
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if(!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
