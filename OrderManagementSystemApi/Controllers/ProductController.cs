using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Product;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        [HttpPost("CreateProduct")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            var response = await _productService.CreateProductAsync(productDto);

            return Ok(response);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);

            return Ok(response);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);

            return Ok(response);
        }
        [HttpGet("GetAllProducts")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<List<ProductResponseDto>>>> GetAllProducts()
        {
            var response = await _productService.GetAllProductsAsync();

            return Ok(response);
        }
        [HttpGet("GetAllProductsByCategory/{categoryId}")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<List<ProductResponseDto>>>> GetAllProductsByCategory(int categoryId)
        {
            var response = await _productService.GetAllProductsByCategoryAsync(categoryId);

            return Ok(response);
        }
        [HttpPut("UpdateProductStatus")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdateProductStatus(ProductStatusUpdateDto productStatusUpdateDTO)
        {
            var response = await _productService.UpdateProductStatusAsync(productStatusUpdateDTO);

            return Ok(response);
        }
    }
}
