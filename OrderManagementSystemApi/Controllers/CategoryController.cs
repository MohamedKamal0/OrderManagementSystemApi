using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.Category;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            var response = await _categoryService.CreateCategoryAsync(categoryDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ApiResponse<List<CategoryResponseDto>>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}

