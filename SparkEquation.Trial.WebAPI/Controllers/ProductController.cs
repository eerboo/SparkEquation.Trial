using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparkEquation.Trial.WebAPI.Model;
using SparkEquation.Trial.WebAPI.Model.Request;
using SparkEquation.Trial.WebAPI.Services;
using System.Threading.Tasks;

namespace SparkEquation.Trial.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;

        public ProductController(IProductsService productsService, IMapper mapper)
        {
            _productsService = productsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = _productsService.GetAllProductsData();
            return new JsonResult(results);
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ProductDto), 200)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _productsService.GetProductByIdAsync(id);

            if (result == null) return NotFound();

            var res = _mapper.Map<ProductDto>(result);
            return Ok(res);
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 200)]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productsService.AddProductAsync(request);

            var res = _mapper.Map<ProductDto>(result);
            return CreatedAtAction("Add", new { id = res.Id }, res);
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(ProductDto), 200)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productsService.UpdateProductAsync(request);
            if (result == null) return NotFound();

            var res = _mapper.Map<ProductDto>(result);

            return Ok(res);
        }

        /// <summary>
        /// Delete product by product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("remove/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var res = await _productsService.RemoveProductAsync(id);
            if (res) return Ok();
            return NotFound();
        }
    }
}