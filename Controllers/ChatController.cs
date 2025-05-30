using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatzyWebClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ChatController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequest request)
        {
            var endpoint = "https://calvarygtp1.openai.azure.com/openai/deployments/CalvaryGTP1/chat/completions?api-version=2024-02-15-preview";
            OPENAI_API_KEY = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY");

            var body = new
            {
                messages = new[]
                {
                    new { role = "system", content = "Eres un tutor educativo. Ayuda a los estudiantes a entender sus tareas sin darles la respuesta directamente." },
                    new { role = "user", content = request.Prompt }
                }
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
            httpRequest.Headers.Add("api-key", apiKey);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            var responseBody = await response.Content.ReadAsStringAsync();

            return Content(responseBody, "application/json");
        }
    }

    public class ChatRequest
    {
        public string Prompt { get; set; }
    }
}
