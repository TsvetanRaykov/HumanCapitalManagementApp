using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace HCM.App.Pages
{
    [Authorize]
    public class ApiModel : PageModel
    {
        public string? Data { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public ApiModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));

            Data = await httpClient.GetStringAsync("https://api:7001/WeatherForecast");
        }
    }
}
