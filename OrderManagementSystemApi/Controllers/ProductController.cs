using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            var response = await _productService.CreateProductAsync(productDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves a product by ID.
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        
        // Deletes a product by ID.
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves all products.
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ApiResponse<List<ProductResponseDto>>>> GetAllProducts()
        {
            var response = await _productService.GetAllProductsAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves all products by category.
        [HttpGet("GetAllProductsByCategory/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<ProductResponseDto>>>> GetAllProductsByCategory(int categoryId)
        {
            var response = await _productService.GetAllProductsByCategoryAsync(categoryId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Update Product Status
        [HttpPut("UpdateProductStatus")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdateProductStatus(ProductStatusUpdateDto productStatusUpdateDTO)
        {
            var response = await _productService.UpdateProductStatusAsync(productStatusUpdateDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
