using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly List<ProductDTO> _products;

        public ProductsController()
        {
            _products = new List<ProductDTO>
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "Samsung Note 20 Plus"
                }
            };
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if(product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
    }
}