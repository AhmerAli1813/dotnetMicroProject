using Microsoft.AspNetCore.Mvc;
using Shopping.API.DTO;
using Shopping.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _services;
        private  ResponseDto _response;
        public CategoryController(ICategoryServices services)
        {
            _services = services;
            _response = new ResponseDto();
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<object>? Get()
        {
            try
            {
                _response.Result = await _services.GetAllCategory();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Error = new List<string> { ex.Message };
            }
            return  _response;
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                _response.Result = await _services.GetCategoryById(id);
                _response.Message = "success";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Error = new List<string> { ex.Message };
            }
            return _response;
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryDto req)
        {
            try
            {
                await _services.CreateUpdateCategoryAsync(req);
                    
                _response.Message = StatusCode(StatusCodes.Status201Created).ToString();
            }
            catch (Exception ex)
            {
                _response.Error = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryDto req)
        {
            try
            {
                await _services.CreateUpdateCategoryAsync(req);

                _response.Message = StatusCode(StatusCodes.Status201Created).ToString();
            }
            catch (Exception ex)
            {
                _response.Error = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _services.DeleteCategory(id);
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
