using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.DTO;
using Shopping.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProductController : ControllerBase
    {
        private readonly IProductServices _services;
        private ResponseDto _response;
        public ProductController(IProductServices services)
        {
            _services = services;
            _response = new ResponseDto();
        }
        // GET: api/<ProductController>
        
        [HttpGet]

        public async Task<object>? Get()
        {
            try
            {
                _response.Result = await _services.GetAllProduct();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Error = new List<string> { ex.Message };
            }
            return _response;
        }
        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                _response.Result = await _services.GetProductById(id);
                _response.Message = "success";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Error = new List<string> { ex.Message };
            }
            return _response;
        }

        // POST api/<ProductController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] ProductDto req)
        {
            try
            {
                await _services.CreateUpdateProductAsync(req);

                _response.Message = StatusCode(StatusCodes.Status201Created).ToString();
            }
            catch (Exception ex)
            {
                _response.Error = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDto req)
        {
            try
            {
                await _services.CreateUpdateProductAsync(req);

                _response.Message = StatusCode(StatusCodes.Status201Created).ToString();
            }
            catch (Exception ex)
            {
                _response.Error = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _services.DeleteProduct(id);
                _response.Message = StatusCode(StatusCodes.Status202Accepted).ToString();
            }
            catch (Exception ex)
            {
                _response.Error = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
