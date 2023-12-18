using System.Collections.Generic;
using System.Threading.Tasks;
using HelperStockBeta.Application.DTOs;
using HelperStockBeta.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelperStockBeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _productService.GetProducts();
            
            if (products == null)
            {
                return NotFound("Product was not found");
            }

            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound("Product was not found");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Invalid Body Data");
            }

            await _productService.Add(productDto);

            return new CreatedAtRouteResult("GetProduct", new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("Id was not verified");
            }

            if (productDto == null)
            {
                return BadRequest("Invalid Body Data");
            }

            await _productService.Update(productDto);

            return Ok(productDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound("Product was not found");
            }

            await _productService.Remove(id);

            return Ok("Product removed successfully");
        }
    }
}
