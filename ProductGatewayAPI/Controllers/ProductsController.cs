using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductGatewayAPI.Models;

namespace ProductGatewayAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            string productAPIUrl = _configuration.GetValue<string>("productAPIUrl");
            string stockAPIUrl = _configuration.GetValue<string>("stockAPIUrl");

            productAPIUrl = $"{productAPIUrl}/products/{id}";
            stockAPIUrl = $"{stockAPIUrl}/stocks?productId={id}"; ;

            Task<HttpResponseMessage> productResponse = httpClient.GetAsync(productAPIUrl);
            Task<HttpResponseMessage> stockResponse = httpClient.GetAsync(stockAPIUrl);

            await Task.WhenAll(productResponse, stockResponse);

            if (productResponse.Result.IsSuccessStatusCode && stockResponse.Result.IsSuccessStatusCode)
            {
                var jOption = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                using var productResponseContent = await productResponse.Result.Content.ReadAsStreamAsync();
                ProductDTO product = await JsonSerializer.DeserializeAsync<ProductDTO>(productResponseContent, jOption);

                using var stockResponseContent = await stockResponse.Result.Content.ReadAsStreamAsync();
                StockDTO stock = await JsonSerializer.DeserializeAsync<StockDTO>(stockResponseContent, jOption);

                var aggregatedProduct = new ProductAggregatedDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Quantity = stock.Quantity
                };

                return Ok(aggregatedProduct);
            };

            return NotFound();
        }
    }
}