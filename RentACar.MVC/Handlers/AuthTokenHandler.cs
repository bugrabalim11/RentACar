using System.Net.Http.Headers;

namespace RentACar.MVC.Handlers
{
    // 1. DelegatingHandler'dan miras aldık (Artık bu sınıf bir gümrük memuru)
    public class AuthTokenHandler : DelegatingHandler
    {
        // 2. Cüzdana ulaşmak için Maymuncuk Anahtarı tanımladık
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // 3. Memurun asıl işi yapacağı Gümrük Kapısı metodu (Birazdan içini dolduracağız)
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Cüzdanı aç ve "AccessToken" etiketli kartı bulmaya çalış
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            // 2. Eğer cüzdanda kart (token) boş değilse:
            if(!string.IsNullOrEmpty(token))
            {
                // Kuryenin (request) yakasına (Headers.Authorization) bu kartı "Bearer" olarak tak!
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 3. Kuryeyi API'ye doğru yola sal
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
