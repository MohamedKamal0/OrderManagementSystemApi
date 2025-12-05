using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.Category;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Enums;
using OrderManagementSystemInfrastructure.Authorization;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        [HttpPost("CreateCategory")]
        [CheckPermission(Permission.Write)]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            var response = await _categoryService.CreateCategoryAsync(categoryDto);

            return Ok(response);
        }
        [HttpGet("GetAllCategories")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<List<CategoryResponseDto>>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();

            return Ok(response);
        }
    }
}

