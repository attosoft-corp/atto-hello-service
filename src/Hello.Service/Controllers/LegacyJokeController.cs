using System.Net.Http;
using System.Threading.Tasks;
using Hello.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hello.Service.Controllers
{
    [Route("api/[controller]")]
    public class LegacyJokeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LegacyJokeController> _logger;


        public LegacyJokeController(IHttpClientFactory httpClientFactory, ILogger<LegacyJokeController> logger)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<ActionResult<JokeResponse>> GetJokeAsync()
        {
            var response = await _httpClient.GetAsync("https://api.icndb.com/jokes/random");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(content);
                return jokeResponse;
            }

            throw new System.Exception("erro requesting the joke");
        }

    }
}