using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly List<StockDTO> _stocks;

        public StocksController()
        {
            _stocks = new List<StockDTO>
            {
                new StockDTO
                {
                    Id = 1,
                    ProductId = 1,
                    Quantity = 5
                }
            };
        }

        [HttpGet]
        public IActionResult Get([FromQuery]int productId)
        {
            var stock = _stocks.FirstOrDefault(x => x.ProductId == productId);
            if(stock != null)
            {
                return Ok(stock);
            }
            else
            {
                return NotFound();
            }
        }
    }
}